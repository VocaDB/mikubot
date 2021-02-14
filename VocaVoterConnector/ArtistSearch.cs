using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using VocaDb.Model.Service;

namespace MikuBot.VocaDBConnector
{
	public class ArtistSearch : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var matchMode = NameMatchMode.Auto;
			string query = SearchHelper.GetSearchQuery(cmd.BotCommand, ref matchMode);

			var artists = connectorFile.CallClient(client => client.FindArtists(query, 5, matchMode));

			if (!artists.Items.Any())
			{
				reply.Msg("No results");
				return;
			}

			reply.Msg("Found " + artists.TotalCount + " result(s) (displaying first 5).");
			var config = connectorFile.Config;

			foreach (var artist in artists.Items)
			{
				var nameStr = EntryFormattingHelper.FormatArtistWithUrl(artist, config);

				reply.Msg(nameStr);
			}
		}

		public override string Name
		{
			get { return "Artistsearch"; }
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
			get { return "Searches artists by name from VocaDB"; }
		}

		public override string UsageHelp
		{
			get { return "artistsearch <term>"; }
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
