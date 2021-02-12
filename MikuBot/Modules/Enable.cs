using MikuBot.Commands;

namespace MikuBot.Modules
{
	public class Enable : BuiltinModule
	{
		public override string HelpText
		{
			get { return "Enables a plugin module."; }
		}

		public override void HandleCommand(MsgCommand cmd, Bot bot)
		{
			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			if (!CheckAccess(cmd, bot))
				return;

			var moduleName = cmd.BotCommand.Params[0];
			var success = bot.ModuleManager.EnablePluginModule(moduleName);
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			if (success)
			{
				receiver.Msg("Module '" + moduleName + "' has been enabled");
			}
			else
			{
				receiver.Msg("Module '" + moduleName + "' not found");
			}
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Manager; }
		}

		public override string Name
		{
			get { return "Enable"; }
		}
	}
}
