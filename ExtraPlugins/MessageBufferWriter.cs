using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class MessageBufferWriter : MsgCommandModuleBase
	{
		private MessageBuffer messageBuffer;

		public override string HelpText
		{
			get { return "Logs messages to buffer."; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "MessageBufferWriter"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			messageBuffer.Record(cmd.ChannelOrSenderNick, cmd);
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			if (moduleFile == null)
				return;

			messageBuffer = ((ExtraPluginsModuleFile)moduleFile).MessageBuffer;
		}
	}
}
