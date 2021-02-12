using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Commands : MsgCommandModuleBase
	{
		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!cmd.BotCommand.Is(Name))
				return;

			var sender = new Receiver(bot.Writer, cmd.Sender.Nick);

			Action<string, IEnumerable<IModule>> PrintCommands =
				delegate (string title, IEnumerable<IModule> modules)
				{
					var commands = modules.Where(m => m.MinUserLevel <= cmd.Sender.UserLevel)
						.Select(m => (bot.ModuleManager.IsModuleEnabled(m) ? m.Name : "(" + m.Name + ")"));

					sender.Notice(Formatting.Bold + title);

					var cmdString = string.Join(" ", commands);
					sender.Notice(cmdString);
				};

			var moduleManager = bot.ModuleManager;

			if (cmd.BotCommand.Params.HasParam(0))
			{
				var cmdName = cmd.BotCommand.Params[0];

				var module = moduleManager.FindModule(cmdName);

				if (module == null)
				{
					sender.Notice("No command found with the name '" + cmdName + "'.");
					return;
				}

				sender.Notice(Formatting.Bold + module.Name);

				if (string.IsNullOrEmpty(module.HelpText))
				{
					sender.Notice("No help available.");
					return;
				}

				var lines = module.HelpText.Split('\n');

				foreach (var line in lines)
					sender.Notice(line);
			}
			else
			{
				var helpLink = bot.Config.HelpLink;
				var helpText = !string.IsNullOrEmpty(helpLink) ? ", full help available at " + helpLink : string.Empty;

				var msg = string.Format("MikuBot v{0}, {1} modules{2}",
					Application.ProductVersion, moduleManager.AllModules.Count(), helpText);

				var commandModules = moduleManager.BuiltinModules.Concat(moduleManager.MsgCommandModules.Where(m => !m.IsPassive)).OrderBy(c => c.Name);

				PrintCommands(msg, commandModules);
			}
		}

		public override string CommandDescription
		{
			get { return "Displays a list of commands."; }
		}

		public override string UsageHelp
		{
			get { return "commands [<command name>]"; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Unidentified; }
		}

		public override string Name
		{
			get { return "Commands"; }
		}
	}
}
