using MikuBot.Modules;
using MikuBot.Commands;
using MikuBot.VocaDBConnector.Helpers;

namespace MikuBot.VocaDBConnector
{
	public class TagDetails : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var query = cmd.BotCommand.CommandString.Replace(' ', '_');
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			var tag = connectorFile.CallClient(client => client.GetTagByName(query));

			if (tag == null)
			{
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatTagWithUrl(tag, connectorFile.Config));
		}

		public override string Name
		{
			get { return "Tag"; }
		}

		public override string CommandDescription
		{
			get
			{
				return "Displays details for a single tag matching a query.";
			}
		}

		public override string UsageHelp
		{
			get
			{
				return "tag <query>";
			}
		}

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}
	}
}
