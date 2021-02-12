using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector
{
	public class AlbumSearch : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var matchMode = NameMatchMode.Auto;
			string query = SearchHelper.GetSearchQuery(cmd.BotCommand, ref matchMode);

			var albums = connectorFile.CallClient(client => client.FindAlbums(query, 5, matchMode, AlbumSortRule.NameThenReleaseDate));

			if (!albums.Items.Any())
			{
				reply.Msg("No results");
				return;
			}

			reply.Msg("Found " + albums.TotalCount + " result(s) (displaying first 5).");
			var config = connectorFile.Config;

			foreach (var album in albums.Items)
			{
				reply.Msg(EntryFormattingHelper.FormatAlbumWithUrl(album, config));
			}
		}

		public override string Name
		{
			get { return "AlbumSearch"; }
		}

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 10000; }
		}

		public override string CommandDescription
		{
			get { return "Searches albums by name from VocaDB"; }
		}

		public override string UsageHelp
		{
			get { return "albumsearch <term>"; }
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
