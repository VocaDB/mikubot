using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector {

	public class AlbumSearchAdvanced : MsgCommandModuleBase {

		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var matchMode = NameMatchMode.Auto;
			string query = SearchHelper.GetSearchQuery(cmd.BotCommand, ref matchMode);

			var albums = connectorFile.CallClient(client => client.FindAlbumsAdvanced(query, 5));

			if (!albums.Items.Any()) {
				reply.Msg("No results");
				return;
			}

			reply.Msg("Found " + albums.TotalCount + " result(s) (displaying first 5).");
			var config = connectorFile.Config;

			foreach (var album in albums.Items) {
				reply.Msg(EntryFormattingHelper.FormatAlbumWithUrl(album, config));
			}

		}

		public override string Name {
			get { return "AlbumSearchAdv"; }
		}

		public override int CooldownChannelMs {
			get { return 0; }
		}

		public override int CooldownUserMs {
			get { return 0; }
		}

		public override string CommandDescription {
			get { return "Searches albums by title, artist or tag"; }
		}

		public override string UsageHelp {
			get { return "albumsearchadv <query>"; }
		}

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			connectorFile = (VocaVoterConnectorFile)moduleFile;

		}

	}

}
