using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector
{
	public class SongSearch : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var matchMode = NameMatchMode.Auto;
			string query = SearchHelper.GetSearchQuery(cmd.BotCommand, ref matchMode, false);

			// Enable advanced parser with "!"
			var songs = connectorFile.CallClient(client => client.FindSongs("!" + query, 5, matchMode));

			if (!songs.Items.Any())
			{
				reply.Msg("No results");
				return;
			}

			reply.Msg("Found " + songs.TotalCount + " results (displaying first 5)");

			var config = connectorFile.Config;

			foreach (var song in songs.Items)
			{
				reply.Msg(EntryFormattingHelper.FormatSongWithUrl(song, config));
			}
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override string CommandDescription
		{
			get { return "Searches songs by name from VocaDB"; }
		}

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 10000; }
		}

		public override string Name
		{
			get { return "Songsearch"; }
		}

		public override string UsageHelp
		{
			get { return "songsearch <term>"; }
		}
	}
}
