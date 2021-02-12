using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector
{
	public class VocaDbParser : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		private readonly Regex[] linkMatchers = {
			new Regex(@"((?:vocadb\.net)|(?:utaitedb\.net))/(Album|Artist|Song|SongList|Tag|User)/Details/(\d+)"),
			new Regex(@"((?:vocadb\.net)|(?:utaitedb\.net))/(S|Al|Ar|L|T)/(\d+)"),
		};

		private void GetEntryInfo(Receiver receiver, string entryTypeName, string entryId, ClientType clientType)
		{
			switch (entryTypeName)
			{
				case "Al":
					GetAlbumInfo(receiver, int.Parse(entryId), clientType);
					break;

				case "Album":
					GetAlbumInfo(receiver, int.Parse(entryId), clientType);
					break;

				case "Ar":
					GetArtistInfo(receiver, int.Parse(entryId), clientType);
					break;

				case "Artist":
					GetArtistInfo(receiver, int.Parse(entryId), clientType);
					break;

				case "S":
				case "Song":
					GetSongInfo(receiver, int.Parse(entryId), clientType);
					break;

				case "L":
				case "SongList":
					var list = connectorFile.CallClient(client => client.GetSongListById(int.Parse(entryId)));

					if (list == null)
					{
						receiver.Msg("No results.");
						return;
					}

					if (list.FeaturedCategory == SongListFeaturedCategory.Nothing)
						receiver.Msg(string.Format("Song list: {0} by {1}", list.Name, list.Author.Name));
					else
						receiver.Msg(string.Format("Song list: {0} ({1})", list.Name, list.FeaturedCategory));

					break;

				case "T":
				case "Tag":
					var tag = connectorFile.CallClient(client => client.GetTagById(int.Parse(entryId), ContentLanguagePreference.English));

					if (tag == null)
					{
						receiver.Msg("No results.");
						return;
					}

					receiver.Msg(EntryFormattingHelper.FormatTag(tag));
					break;
			}
		}

		private void GetAlbumInfo(Receiver receiver, int albumId, ClientType clientType)
		{
			var album = connectorFile.CallClient(clientType, client => client.GetAlbumById(albumId));

			if (album == null)
			{
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatAlbum(album));
		}

		private void GetArtistInfo(Receiver receiver, int artistId, ClientType clientType)
		{
			var artist = connectorFile.CallClient(clientType, client => client.GetArtistById(artistId));

			if (artist == null)
			{
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatArtist(artist));
		}

		private void GetSongInfo(Receiver receiver, int songId, ClientType clientType)
		{
			var song = connectorFile.CallClient(clientType, client => client.GetSongById(songId, ContentLanguagePreference.English));

			if (song == null)
			{
				receiver.Msg("No results.");
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatSong(song.Song));
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}

		public override string HelpText
		{
			get { return "Parses VocaDB links. By prefixing the link with 'nolink', all link parsing is skipped. This is useful if you don't want some link to be parsed."; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "VocaDbParser"; }
		}

		public override string UsageHelp
		{
			get { return "[<nolink>] <VocaDB URL>"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (cmd.BotCommand.Is("NoLink"))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			var matcher = linkMatchers.FirstOrDefault(m => m.IsMatch(cmd.Text));

			if (matcher == null)
				return;

			var match = matcher.Match(cmd.Text);
			var domain = match.Groups[1].Value;
			var clientType = string.Equals(domain, "utaitedb.net") ? ClientType.UtaiteDb : ClientType.VocaDb;
			var entryTypeName = match.Groups[2].Value;
			var entryId = match.Groups[3].Value;

			Task.Factory.StartNew(() => GetEntryInfo(receiver, entryTypeName, entryId, clientType))
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);
		}
	}
}
