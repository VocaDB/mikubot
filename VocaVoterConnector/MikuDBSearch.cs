using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector;

namespace MikuBot.VocaVoterConnector {

	public class MikuDBSearch : MsgCommandModuleBase {

		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var query = cmd.BotCommand.CommandString;

			var urls = connectorFile.VocaDbClient.FindMikuDB(query);

			if (urls == null) {
				reply.Msg("No results");
				return;
			}

			reply.Msg("MikuDB: " + urls);

		}

		public override string Name {
			get { return "MikuDBSearch"; }
		}

		public override int CooldownChannelMs {
			get { return 1000; }
		}

		public override int CooldownUserMs {
			get { return 10000; }
		}

		public override string CommandDescription {
			get { return "Searches albums by name from MikuDB"; }
		}

		public override string UsageHelp {
			get { return "mikudbsearch <term>"; }
		}

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			connectorFile = (VocaVoterConnectorFile)moduleFile;

		}

	}

}
