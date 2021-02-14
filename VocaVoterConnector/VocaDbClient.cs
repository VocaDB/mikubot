using System;
using System.Threading.Tasks;
using com.cardinal.voca4j;
using com.cardinal.voca4j.api;
using com.cardinal.voca4j.api.entities.song;
using com.cardinal.voca4j.api.song;
using com.cardinal.voca4j.api.song.get;
using com.cardinal.voca4j.impl;
using VocaDb.Model.DataContracts.Albums;
using VocaDb.Model.DataContracts.Api;
using VocaDb.Model.DataContracts.Artists;
using VocaDb.Model.DataContracts.Songs;
using VocaDb.Model.DataContracts.Tags;
using VocaDb.Model.DataContracts.Users;
using VocaDb.Model.Domain.Globalization;
using VocaDb.Model.Service;
using VocaDbNameMatchMode = VocaDb.Model.Service.NameMatchMode;
using VocaDbPVService = VocaDb.Model.Domain.PVs.PVService;
using VocaDbSongType = VocaDb.Model.Domain.Songs.SongType;

namespace MikuBot.VocaDBConnector
{
	internal static class APIServiceExtensions
	{
		public static T completeAndParse<T>(this APIService apiService, Request request) => (T)apiService.completeAndParseGenerically(request);
	}

	internal static class BuilderProviderExtensions
	{
		public static T getBuilder<T>(this BuilderProvider builderProvider) where T : StateControlledBuilder => (T)builderProvider.getBuilder(typeof(T));
	}

	public sealed class VocaDbClient : IDisposable
	{
		private readonly string _endpoint;
		private readonly Voca4J _voca4j = new();

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
			VocaDbNameMatchMode nameMatchMode = VocaDbNameMatchMode.Auto, AlbumSortRule sort = AlbumSortRule.NameThenReleaseDate)
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

		public PartialFindResult<ArtistContract> FindArtists(string term, int maxResults, VocaDbNameMatchMode nameMatchMode = VocaDbNameMatchMode.Auto)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<SongWithAlbumAndPVsContract> FindSongs(string term, int maxResults, VocaDbNameMatchMode nameMatchMode = VocaDbNameMatchMode.Auto)
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
			var builder = _voca4j.getBuilderProvider().getBuilder<SongGet.SongGetBuilder>();
			var request = builder
				.setID(id)
				.setLanguage(Language.valueOf(language.ToString()))
				.setFields(new[] { SongField.AdditionalNames })
				.build();
			Constants.rootURL = _endpoint;
			var song = _voca4j.getApiService().completeAndParse<Song>(request);

			return new SongDetailsContract
			{
				Song = new SongContract
				{
					AdditionalNames = song.getAdditionalNames(),
					ArtistString = song.getArtistString(),
					Id = song.getId(),
					LengthSeconds = song.getLengthSeconds(),
					Name = song.getName(),
					PublishDate = DateTime.TryParse(song.getPublishDate().toString().ToString(), out var publishDate) ? publishDate : null,
					RatingScore = song.getRatingScore(),
					SongType = Enum.TryParse<VocaDbSongType>(song.getSongType().name(), out var songType) ? songType : VocaDbSongType.Unspecified,
				},
			};
		}

		public SongDetailsContract GetSongDetails(string term, ContentLanguagePreference? language = null, VocaDbNameMatchMode matchMode = VocaDbNameMatchMode.Auto)
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

		public Task<SongWithAlbumContract> GetSongWithPVAsync(VocaDbPVService service, string pvId)
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
