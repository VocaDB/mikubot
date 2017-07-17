using MikuBot.Commands;

namespace MikuBot.Modules {

	public abstract class MsgCommandModuleBase : ModuleBase, IMsgCommandModule {

		private CooldownTimer channelCooldown;
		private CooldownTimer userCooldown;

		protected MsgCommandModuleBase() {

			channelCooldown = new CooldownTimer();
			userCooldown = new CooldownTimer();

		}

		private string UsageText {
			get { return Formatting.Bold + "Usage: " + Formatting.Bold + UsageHelp; }
		}

		public virtual int BotCommandParamCount {
			get { return 0; }
		}

		public virtual int CooldownChannelMs {
			get { return 0; }
		}

		public virtual int CooldownUserMs {
			get { return 0; }
		}

		public virtual string CommandDescription {
			get { return string.Empty; }
		}

		public override string HelpText {
			get {
				return
					(!string.IsNullOrEmpty(CommandDescription) ? CommandDescription : string.Empty)
					+ (!string.IsNullOrEmpty(UsageHelp) && !string.IsNullOrEmpty(CommandDescription) ? "\n" : string.Empty)
					+ (!string.IsNullOrEmpty(UsageHelp) ? UsageText : string.Empty);
			}
		}

		public virtual bool IsPassive {
			get { return false; }
		}

		public virtual string UsageHelp {
			get { return string.Empty; }
		}

		public abstract void HandleCommand(MsgCommand cmd, IBotContext bot);

		protected virtual bool CheckCall(MsgCommand command, IBotContext bot) {

			if (!command.BotCommand.Is(Name))
				return false;

			if (!CheckAccess(command, bot))
				return false;

			if (command.BotCommand.Params.Count < BotCommandParamCount) {

				bot.Writer.Msg(command.ChannelOrSenderNick, string.Format("The minimum number of parameters for '{0}' is {1}.", 
					Name, BotCommandParamCount));

				if (!string.IsNullOrEmpty(UsageHelp))
					bot.Writer.Msg(command.ChannelOrSenderNick, UsageText);

				return false;

			}

			if (!CheckCooldowns(command, bot, true))
				return false;

			return true;

		}

		protected bool CheckCooldowns(MsgCommand command, IBotContext bot, bool warn) {

			if (!userCooldown.CheckAccess(command.Sender.Nick, command.Sender.Nick, bot, CooldownUserMs, warn))
				return false;

			if (!channelCooldown.CheckAccess(command.ChannelOrSenderNick, command.Sender.Nick, bot, CooldownChannelMs, warn))
				return false;

			return true;

		}

		public void ClearCooldown(MsgCommand command) {
			
			userCooldown.Clear(command.Sender.Nick);
			channelCooldown.Clear(command.ChannelOrSenderNick);

		}

	}

}
