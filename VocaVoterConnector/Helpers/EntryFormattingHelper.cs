using System;
using System.Linq;
using MikuBot.VocaDBConnector.VocaDbServices;
using System.Text;
using MikuBot.Helpers;
using MikuBot.VocaVoterConnector;

namespace MikuBot.VocaDBConnector.Helpers {

	public static class EntryFormattingHelper {

		public static string FormatAlbum(AlbumContract album) {

			var sb = new StringBuilder(string.Format("Album: {0}{1}{0} ({2}){3}",
				Formatting.Bold,
				FormatName(album.Name, album.AdditionalNames) + (!string.IsNullOrEmpty(album.ArtistString) ? " by " + album.ArtistString : string.Empty),
				album.DiscType,
				album.ReleaseDate != null && !album.ReleaseDate.IsEmpty ? ", released " + album.ReleaseDate.Formatted : string.Empty));

			return sb.ToString();

		}

		public static string FormatAlbumWithUrl(AlbumContract album, VocaDbConfig config, ClientType site = ClientType.VocaDb) {

			return string.Format("{0} - {1}", FormatAlbum(album), GetAlbumUrl(config, album.Id, site));

		}

		public static string FormatArtist(ArtistContract artist) {

			return string.Format("Artist: {0}{1}{0} ({2})",
				Formatting.Bold,
				FormatName(artist.Name, artist.AdditionalNames),
				artist.ArtistType);

		}

		public static string FormatArtistWithUrl(ArtistContract artist, VocaDbConfig config) {

			return string.Format("{0} - {1}", FormatArtist(artist), GetArtistUrl(config, artist.Id));

		}

		public static string FormatEntryWithUrl(EntryForApiContract entry, VocaDbConfig config) {

			return string.Format("Result: {0}{1}{0} ({2})",
				Formatting.Bold,
				FormatName(entry.Name, entry.AdditionalNames),
				entry.ArtistType?.ToString() ?? entry.DiscType?.ToString() ?? entry.SongType?.ToString());

		}

		public static string FormatName(string name, string additionalNames) {

			ParamIs.NotNull(() => name);

			var nameStr = (!string.IsNullOrEmpty(additionalNames)
				? string.Format("{0} ({1})", name, additionalNames)
				: name);

			return nameStr;

		}

		public static string FormatSong(SongContract song) {

			var length = song.LengthSeconds > 0 ? TimeSpan.FromSeconds(song.LengthSeconds) : TimeSpan.Zero;

			return string.Format("Song: {0}{1}{0} ({2}){3}{4}, {5} score",
				Formatting.Bold,
				FormatName(song.Name, song.AdditionalNames) + (!string.IsNullOrEmpty(song.ArtistString) ? " by " + song.ArtistString : string.Empty),
				song.SongType,
				length != TimeSpan.Zero ? string.Format(" ({0}:{1}{2})", (int)length.TotalMinutes, length.Seconds < 10 ? "0" : string.Empty, length.Seconds) : string.Empty,
				song.PublishDate.HasValue ? ", published " + song.PublishDate.Value.ToShortDateString() : string.Empty,
				song.RatingScore
			);

		}

		/*public static string FormatSong(SongDetailsContract song, bool genreTags) {

			var genreTags = song.Tags.Where(t => t.Tag.Ca)

			return
				FormatSong(song.Song) +
				(genreTags && song.Tags);

		}*/

		public static string FormatSongWithUrl(SongContract song, VocaDbConfig config, ClientType site = ClientType.VocaDb) {

			return string.Format("{0} - {1}", FormatSong(song), GetSongUrl(config, song.Id));

		}

		public static string FormatSongWithUrl(SongContract song, NicoApi.VideoDataResult nicoData, VocaDbConfig config, ClientType site = ClientType.VocaDb) {

			return string.Format("{0} - {1} - Nico video uploaded at {2} by {3}, {4} views", 
				FormatSong(song), 
				GetSongUrl(config, song.Id, site),
				nicoData.Created.ToString("g"), nicoData.Author, nicoData.Views);

		}

		public static string FormatSongWithAlbumAndUrl(SongWithAlbumContract song, VocaDbConfig config, ClientType site = ClientType.VocaDb) {

			if (song.Album == null)
				return FormatSongWithUrl(song, config, site);

			return string.Format("{0} from album {1} - {2}", FormatSong(song), song.Album.Name, GetSongUrl(config, song.Id, site));

		}

		public static string FormatSongWithAlbumAndUrl(SongWithAlbumContract song, NicoApi.VideoDataResult nicoData, VocaDbConfig config, ClientType site = ClientType.VocaDb) {

			if (song.Album == null)
				return FormatSongWithUrl(song, nicoData, config, site);

			return string.Format("{0} from album {1} - {2} - Nico video uploaded at {3} by {4}, {5} views", 
				FormatSong(song), 
				song.Album.Name,
				GetSongUrl(config, song.Id, site),
				nicoData.Created.ToString("g"), nicoData.Author, nicoData.Views);

		}

		public static string FormatTag(TagContract tag) {

			var sb = new StringBuilder(string.Format("Tag: {0}{1}{0}", 
				Formatting.Bold, 
				FormatName(tag.Name, tag.AdditionalNames)));

			if (!string.IsNullOrEmpty(tag.CategoryName))
				sb.Append(" (" + tag.CategoryName + ")");

			if (!string.IsNullOrEmpty(tag.Description))
				sb.Append(" - " + tag.Description.Truncate(100));

			return sb.ToString();

		}

		public static string FormatTagWithUrl(TagContract tag, VocaDbConfig config) {

			return string.Format("{0} - {1}", FormatTag(tag), GetTagUrl(config, tag.Name));

		}

		public static string GetAlbumUrl(VocaDbConfig config, int id, ClientType site = ClientType.VocaDb) {
			return new Uri(new Uri(config.GetSiteUrl(site)), "/Al/").ToString() + id;
		}

		public static string GetArtistUrl(VocaDbConfig config, int id, ClientType site = ClientType.VocaDb) {
			return new Uri(new Uri(config.GetSiteUrl(site)), "/Ar/").ToString() + id;
		}

		public static string GetSongUrl(VocaDbConfig config, int id, ClientType site = ClientType.VocaDb) {
			return new Uri(new Uri(config.GetSiteUrl(site)), "/S/").ToString() + id;
		}

		public static string GetTagUrl(VocaDbConfig config, string name, ClientType site = ClientType.VocaDb) {
			return new Uri(new Uri(config.GetSiteUrl(site)), "/Tag/Details/") + name;
		}

	}
}
