using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector {

	public class Songdetails : MsgCommandModuleBase {

		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var matchMode = NameMatchMode.Auto;
			string query = SearchHelper.GetSearchQuery(cmd.BotCommand, ref matchMode, false);
			var receiver = cmd.Reply(bot.Writer);

			SongDetailsContract song;

			int songId;
			if (int.TryParse(query, out songId)) {
				song = connectorFile.CallClient(client => client.GetSongById(songId, ContentLanguagePreference.English));
			} else {
				song = connectorFile.CallClient(client => client.GetSongDetails(matchMode == NameMatchMode.Auto ? "!" + query : query, null, matchMode));				
			}

			if (song == null) {
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatSongWithUrl(song.Song, connectorFile.Config));

		}

		public override string Name {
			get { return "Song"; }
		}

		public override string CommandDescription {
			get {
				return "Displays details for a single song matching a query.";
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
				return "song <query>";
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
