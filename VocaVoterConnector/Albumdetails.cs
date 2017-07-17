using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector {

	public class Albumdetails : MsgCommandModuleBase {

		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var query = cmd.BotCommand.CommandString;
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			AlbumContract album;
			int albumId;
			if (int.TryParse(query, out albumId)) {
				album = connectorFile.CallClient(client => client.GetAlbumById(albumId));
			} else {
				album = connectorFile.CallClient(client => client.GetAlbumDetails(query, AlbumSortRule.NameThenReleaseDate));
			}

			if (album == null) {
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatAlbumWithUrl(album, connectorFile.Config));

		}

		public override string Name {
			get { return "Album"; }
		}

		public override string CommandDescription {
			get {
				return "Displays details for a single album matching a query.";
			}
		}

		public override int CooldownChannelMs {
			get { return 1000; }
		}

		public override int CooldownUserMs {
			get { return 5000; }
		}

		public override string UsageHelp {
			get {
				return "album <query>";
			}
		}

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			connectorFile = (VocaVoterConnectorFile)moduleFile;

		}

	}

}
