using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class ClearCooldown : MsgCommandModuleBase
	{
		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override string CommandDescription
		{
			get { return "Clears cooldowns for a specific command for the calling user."; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Manager; }
		}

		public override string Name
		{
			get { return "ClearCooldown"; }
		}

		public override string UsageHelp
		{
			get { return "ClearCooldown <command name>"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var commandName = cmd.BotCommand.Params.ParamOrEmpty(0);
			var command = bot.ModuleManager.FindModule(commandName) as MsgCommandModuleBase;

			if (command == null)
			{
				bot.Writer.Msg(cmd.Sender.Nick, "Command not found");
				return;
			}

			command.ClearCooldown(cmd);
		}
	}
}
