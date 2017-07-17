using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Help : MsgCommandModuleBase {

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var sender = new Receiver(bot.Writer, cmd.Sender.Nick);

			Action<string, IEnumerable<IModule>> PrintCommands =
				delegate(string title, IEnumerable<IModule> modules) {

					var commands = modules.Where(m => m.MinUserLevel <= cmd.Sender.UserLevel)
						.Select(m => ( bot.ModuleManager.IsModuleEnabled(m) ? m.Name : "(" + m.Name + ")"));

					sender.Notice(Formatting.Bold + title);

					var cmdString = string.Join(" ", commands);
					sender.Notice(cmdString);

				};

			var moduleManager = bot.ModuleManager;

			if (cmd.BotCommand.Params.HasParam(0)) {

				var cmdName = cmd.BotCommand.Params[0];

				var module = moduleManager.FindModule(cmdName);

				if (module == null) {
					sender.Notice("No command found with the name '" + cmdName + "'.");
					return;
				}

				sender.Notice(Formatting.Bold + module.Name);

				if (string.IsNullOrEmpty(module.HelpText)) {
					sender.Notice("No help available.");
					return;
				}

				var lines = module.HelpText.Split('\n');

				foreach (var line in lines)
					sender.Notice(line);

			} else {

				var helpLink = bot.Config.HelpLink;
				var helpText = !string.IsNullOrEmpty(helpLink) ? ", full help available at " + helpLink : string.Empty;

				sender.Notice(string.Format("MikuBot v{0}{1}", Application.ProductVersion, helpText));
				sender.Notice("A total of " + moduleManager.AllModules.Count() + " modules have been loaded. Shortcut key is " + bot.HighlightShortcut);
				sender.Notice("Type 'HELP [command]' to get command-specific help.");
				sender.Notice("Modules whose name is in parenthesis have been disabled.");

				PrintCommands("Builtin commands: ", moduleManager.BuiltinModules);
				PrintCommands("Module commands: ", moduleManager.MsgCommandModules.Where(m => !m.IsPassive));

				var passive = moduleManager.GenericModules
					.Concat(moduleManager.MsgCommandModules
						.Where(m => m.IsPassive).Cast<IModule>());
				PrintCommands("Passive modules: ", passive);

			}

		}

		public override int CooldownUserMs {
			get { return 10000; }
		}

		public override string CommandDescription {
			get { return "Displays general or command-specific help."; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Unidentified; }
		}

		public override string Name {
			get { return "Help"; }
		}

		public override string UsageHelp {
			get { return "help [<command name>]"; }
		}

	}

}
