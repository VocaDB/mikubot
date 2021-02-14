using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector
{
	public class ArtistDetails : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var query = cmd.BotCommand.CommandString;
			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			ArtistDetailsContract artist;
			if (int.TryParse(query, out var artistId))
			{
				artist = connectorFile.CallClient(client => client.GetArtistById(artistId));
			}
			else
			{
				artist = connectorFile.CallClient(client => client.GetArtistDetails(query));
			}

			if (artist == null)
			{
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatArtistWithUrl(artist, connectorFile.Config));
		}

		public override string Name
		{
			get { return "Artist"; }
		}

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override string CommandDescription
		{
			get
			{
				return "Displays details for a single artist matching a query.";
			}
		}

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 5000; }
		}

		public override string UsageHelp
		{
			get
			{
				return "artist <query>";
			}
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}
	}
}
