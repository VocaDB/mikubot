using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MikuBot.Commands;
using MikuBot.DbPlugins;

namespace MikuBot.Modules {

	public class ModuleManager : IModuleManager {

		private static readonly ILog log = LogManager.GetLogger(typeof(ModuleManager));

		private readonly List<IBuiltinModule> builtinModules = new List<IBuiltinModule> {
			new Authenticate(),
			new Disable(),
			new Enable(),
			new Ignore(),
			new SendRaw(),
			new Quit(),
			new Reload(),
			new Unignore()
		};
		private readonly HashSet<string> disabledModules = new HashSet<string>(new CaseInsensitiveStringComparer());
		private readonly List<IGenericModule> genericModules = new List<IGenericModule>();
		private readonly List<IMsgCommandModule> msgCommandModules = new List<IMsgCommandModule>();

		private IrcCommand currentCommand;

		private void CallModule<T>(T module, Action<T> action) where T : IModule {

			if (!IsPluginEnabled(module))
				return;

			string name = module.Name;

			try {
				action(module);
			} catch (Exception x) {
				log.Error("Module " + name + " caused an unhandled exception", x);
			}

		}

		private T Find<T>(IEnumerable<T> modules, string name) where T : IModule {

			return modules.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

		}

		private TModule Find<T, TModule>(IEnumerable<T> modules) where T : IModule where TModule : T {

			return (TModule)modules.FirstOrDefault(m => m is TModule);

		}

		private void bot_Connected(object sender, EventArgs e) {

			var bot = (Bot)sender;

			foreach (var module in AllModules)
				CallModule(module, m => m.OnConnected(bot));

		}

		private void OnDisconnected(object sender, EventArgs e) {

			var bot = (Bot)sender;

			foreach (var module in GenericModules)
				CallModule(module, m => m.OnDisconnected(bot));
			
		}

		public ModuleManager(Bot bot) {
			
			ParamIs.NotNull(() => bot);

			bot.Disconnected += OnDisconnected;
			bot.Connected += new EventHandler(bot_Connected);

		}

		public IEnumerable<IModule> AllModules {
			get { return builtinModules.Concat(PluginModules); }
		}

		public IEnumerable<IBuiltinModule> BuiltinModules {
			get { return builtinModules; }
		}

		IEnumerable<IModule> IModuleManager.BuiltinModules {
			get { return builtinModules; }
		}

		public IEnumerable<IGenericModule> GenericModules {
			get { return genericModules; }
		}

		public IEnumerable<IMsgCommandModule> MsgCommandModules {
			get { return msgCommandModules; }
		}

		public IEnumerable<IModule> PluginModules {
			get { return genericModules.Concat(msgCommandModules.Cast<IModule>()); }
		}

		public void CallModule(IBotContext bot, IModule module) {

			ParamIs.NotNull(() => bot);
			ParamIs.NotNull(() => module);

			var msgCommand = currentCommand as MsgCommand;

			if (msgCommand != null && module is IMsgCommandModule)
				((IMsgCommandModule)module).HandleCommand(msgCommand, bot);

			if (module is IGenericModule)
				CallModule((IGenericModule)module, m => m.HandleCommand(currentCommand, bot));

		}

		public bool DisablePluginModule(string name, bool force = false) {

			if (force) {
				disabledModules.Add(name);
				return true;
			}

			var module = Find(PluginModules, name);

			if (module != null) {
				disabledModules.Add(module.Name);
				return true;
			} else {
				return false;
			}

			/*var genericModule = Find(genericModules, name);

			if (genericModule != null) {
				dis
				return true;
			}

			var msgCommandModule = Find(msgCommandModules, name);

			if (msgCommandModule != null) {
				msgCommandModules.Remove(msgCommandModule);
				return true;
			}

			return false;*/

		}

		public void DisablePluginModules(IEnumerable<string> names, bool force = false) {

			ParamIs.NotNull(() => names);

			foreach (var name in names)
				DisablePluginModule(name, force);

		}

		public bool EnablePluginModule(string name) {

			var module = Find(PluginModules, name);

			if (module != null) {
				disabledModules.Remove(module.Name);
				return true;
			} else {
				return false;
			}

		}

		public IModule FindModule(string name) {
			return Find(AllModules, name);
		}

		public T FindModule<T>() where T : IModule {
			return Find<IModule, T>(AllModules);
		}

		public void HandleCommand(IrcCommand command, Bot bot) {

			currentCommand = command;
			var msgCommand = command as MsgCommand;

			if (msgCommand != null)
				foreach (var module in builtinModules)
					module.HandleCommand(msgCommand, bot);

			foreach (var module in genericModules)
				CallModule(module, (m => m.HandleCommand(command, bot)));

			if (msgCommand != null)
				foreach (var module in msgCommandModules)
					CallModule(module, (m => m.HandleCommand(msgCommand, bot)));

		}

		public void HandleLine(string ircLine, Bot bot) {

			foreach (var module in genericModules)
				CallModule(module, (m => m.OnMessageReceived(ircLine, bot)));

		}

		public bool IsModuleEnabled(IModule module) {

			ParamIs.NotNull(() => module);

			return (IsPluginEnabled(module) || BuiltinModules.Contains(module));

		}

		public bool IsPluginEnabled(IModule module) {

			ParamIs.NotNull(() => module);

			return (!disabledModules.Contains(module.Name));

		}

		public void LoadModules(IBotContext bot, IReceiver receiver, string[] moduleFiles) {

			//songSearch = new SongSearch();

			genericModules.Clear();
			msgCommandModules.Clear();

			foreach (var moduleFile in moduleFiles) {

				Assembly assembly;

				try {
					byte[] assemblyBytes = File.ReadAllBytes(moduleFile);
					assembly = Assembly.Load(assemblyBytes);
					//assembly = Assembly.LoadFrom(moduleFile);
				} catch (Exception x) {
					log.Error("Failed to load module assembly " + moduleFile, x);
					continue;
				}

				var types = assembly.GetTypes();

				var moduleFileClass = types.FirstOrDefault(t => t.GetInterface("IModuleFile") != null);
				IModuleFile moduleFileObj = null;

				if (moduleFileClass != null) {
					
					try {

						moduleFileObj = (IModuleFile)Activator.CreateInstance(moduleFileClass);
						moduleFileObj.OnLoading(bot);

					} catch (Exception x) {
						log.Error("Failed to instantiate module assembly " + moduleFile + ", " + moduleFileClass, x);
					}

				}

				var moduleClasses = types.Where(t => t.GetInterface("IModule") != null);

				foreach (var moduleClass in moduleClasses) {

					try {
						var instance = (IModule)Activator.CreateInstance(moduleClass);

						if (instance.InitialStatus == InitialModuleStatus.NotLoaded)
							continue;

						if (instance.InitialStatus == InitialModuleStatus.Disabled)
							DisablePluginModule(instance.Name, true);

						if (instance is IMsgCommandModule)
							msgCommandModules.Add((IMsgCommandModule)instance);
						else if (instance is IGenericModule)
							genericModules.Add((IGenericModule)instance);

						instance.OnLoaded(bot, moduleFileObj);

						if (moduleFileObj != null)
							moduleFileObj.OnModuleLoaded(instance, bot);

					} catch (Exception x) {
						log.Error("Failed to instantiate module assembly " + moduleFile + ", " + moduleClass, x);
					}

				}

				if (moduleFileObj != null) {

					try {
						moduleFileObj.OnLoaded(bot);
					} catch (Exception x) {
						log.Error("Failed to instantiate module assembly " + moduleFile + ", " + moduleFileObj, x);
					}
					
				}

			}

			receiver.Msg(string.Format("Successfully loaded {0} module(s) in {1} file(s)", PluginModules.Count(), moduleFiles.Length));

		}

		public void OnMessageSent(string ircLine, Bot bot) {

			ParamIs.NotNull(() => bot);

			foreach (var module in genericModules)
				CallModule(module, (m => m.OnMessageSent(ircLine, bot)));

		}

	}

}
