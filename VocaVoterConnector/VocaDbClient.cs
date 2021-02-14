using System;
using System.Threading.Tasks;
using VocaDb.Model.DataContracts.Albums;
using VocaDb.Model.DataContracts.Api;
using VocaDb.Model.DataContracts.Artists;
using VocaDb.Model.DataContracts.Songs;
using VocaDb.Model.DataContracts.Tags;
using VocaDb.Model.DataContracts.Users;
using VocaDb.Model.Domain.Globalization;
using VocaDb.Model.Domain.PVs;
using VocaDb.Model.Service;

namespace MikuBot.VocaDBConnector
{
	public sealed class VocaDbClient : IDisposable
	{
		private readonly string _endpoint;

		public VocaDbClient(string endpoint)
		{
			_endpoint = endpoint;
		}

		private void Dispose(bool disposing)
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~VocaDbClient() => Dispose(false);

		public PartialFindResult<AlbumContract> FindAlbums(string term, int maxResults,
			NameMatchMode nameMatchMode = NameMatchMode.Auto, AlbumSortRule sort = AlbumSortRule.NameThenReleaseDate)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<AlbumContract> FindAlbumsAdvanced(string term, int maxResults)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<EntryForApiContract> FindAll(string term, int maxResults, ContentLanguagePreference languagePreference)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<ArtistContract> FindArtists(string term, int maxResults, NameMatchMode nameMatchMode = NameMatchMode.Auto)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<SongWithAlbumAndPVsContract> FindSongs(string term, int maxResults, NameMatchMode nameMatchMode = NameMatchMode.Auto)
		{
			throw new NotImplementedException();
		}

		public AlbumDetailsContract GetAlbumById(int id)
		{
			throw new NotImplementedException();
		}

		public AlbumContract GetAlbumDetails(string term, AlbumSortRule sort = AlbumSortRule.NameThenReleaseDate)
		{
			throw new NotImplementedException();
		}

		public ArtistDetailsContract GetArtistById(int id)
		{
			throw new NotImplementedException();
		}

		public ArtistDetailsContract GetArtistDetails(string term)
		{
			throw new NotImplementedException();
		}

		public LyricsForSongContract GetRandomSongLyrics(string query)
		{
			throw new NotImplementedException();
		}

		public SongDetailsContract GetSongById(int id, ContentLanguagePreference? language)
		{
			throw new NotImplementedException();
		}

		public SongDetailsContract GetSongDetails(string term, ContentLanguagePreference? language = null, NameMatchMode matchMode = NameMatchMode.Auto)
		{
			throw new NotImplementedException();
		}

		public SongDetailsContract GetSongDetailsByNameArtistAndAlbum(string name, string artist, string album)
		{
			throw new NotImplementedException();
		}

		public SongListContract GetSongListById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<SongWithAlbumContract> GetSongWithPVAsync(PVService service, string pvId)
		{
			throw new NotImplementedException();
		}

		public TagContract GetTagById(int id, ContentLanguagePreference? language = null)
		{
			throw new NotImplementedException();
		}

		public TagContract GetTagByName(string name)
		{
			throw new NotImplementedException();
		}

		public UserContract GetUser(string name, string accessKey)
		{
			throw new NotImplementedException();
		}
	}
}
