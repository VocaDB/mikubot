using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
		private readonly HttpClient _client = new();

		public VocaDbClient(string endpoint)
		{
			_endpoint = endpoint;
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
				_client.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~VocaDbClient() => Dispose(false);

		private T CallClient<T>(string requestUri)
		{
			// Code from: https://docs.microsoft.com/en-us/archive/blogs/jpsanders/asp-net-do-not-use-task-result-in-main-context
			var stringTask = Task.Run(() => _client.GetStringAsync(requestUri));
			stringTask.Wait();
			var value = stringTask.Result;
			return JsonConvert.DeserializeObject<T>(value);
		}

		public PartialFindResult<AlbumContract> FindAlbums(string term, int maxResults,
			NameMatchMode nameMatchMode = NameMatchMode.Auto, AlbumSortRule sort = AlbumSortRule.NameThenReleaseDate)
		{
			var q = new Dictionary<string, object>
			{
				{ "query", term },
				{ "maxResults", maxResults },
				{ "nameMatchMode", nameMatchMode },
				{ "sort", sort },
				{ "fields", string.Join(",", new[] { AlbumOptionalFields.AdditionalNames }) },
				{ "getTotalCount", true },
			};
			var albums = CallClient<PartialFindResult<AlbumForApiContract>>($"{_endpoint}/albums?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new PartialFindResult<AlbumContract>
			{
				Items = albums.Items.Select(album => new AlbumContract
				{
					AdditionalNames = album.AdditionalNames,
					ArtistString = album.ArtistString,
					DiscType = album.DiscType,
					Id = album.Id,
					Name = album.Name,
					ReleaseDate = album.ReleaseDate,
				}).ToArray(),
				Term = albums.Term,
				TotalCount = albums.TotalCount,
			};
		}

		public PartialFindResult<AlbumContract> FindAlbumsAdvanced(string term, int maxResults)
		{
			throw new NotImplementedException();
		}

		public PartialFindResult<EntryForApiContract> FindAll(string term, int maxResults, ContentLanguagePreference languagePreference)
		{
			var q = new Dictionary<string, object>
			{
				{ "query", term },
				{ "maxResults", maxResults },
				{ "nameMatchMode", NameMatchMode.Auto },
				{ "lang", languagePreference },
				{ "fields", string.Join(",", new[] { EntryOptionalFields.AdditionalNames }) },
				{ "getTotalCount", true },
			};
			return CallClient<PartialFindResult<EntryForApiContract>>($"{_endpoint}/entries?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
		}

		public PartialFindResult<ArtistContract> FindArtists(string term, int maxResults, NameMatchMode nameMatchMode = NameMatchMode.Auto)
		{
			var q = new Dictionary<string, object>
			{
				{ "query", term },
				{ "maxResults", maxResults },
				{ "nameMatchMode", nameMatchMode },
				{ "fields", string.Join(",", new[] { ArtistOptionalFields.AdditionalNames }) },
				{ "getTotalCount", true },
			};
			var artists = CallClient<PartialFindResult<ArtistForApiContract>>($"{_endpoint}/artists?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new PartialFindResult<ArtistContract>
			{
				Items = artists.Items.Select(artist => new ArtistContract
				{
					AdditionalNames = artist.AdditionalNames,
					ArtistType = artist.ArtistType,
					Id = artist.Id,
					Name = artist.Name,
				}).ToArray(),
				Term = artists.Term,
				TotalCount = artists.TotalCount,
			};
		}

		public PartialFindResult<SongWithAlbumAndPVsContract> FindSongs(string term, int maxResults, NameMatchMode nameMatchMode = NameMatchMode.Auto)
		{
			var q = new Dictionary<string, object>
			{
				{ "query", term },
				{ "maxResults", maxResults },
				{ "nameMatchMode", nameMatchMode },
				{ "fields", string.Join(",", new[] { SongOptionalFields.AdditionalNames, SongOptionalFields.Albums }) },
				{ "getTotalCount", true },
			};
			var songs = CallClient<PartialFindResult<SongForApiContract>>($"{_endpoint}/songs?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new PartialFindResult<SongWithAlbumAndPVsContract>
			{
				Items = songs.Items.Select(song => new SongWithAlbumAndPVsContract
				{
					AdditionalNames = song.AdditionalNames,
					ArtistString = song.ArtistString,
					Id = song.Id,
					LengthSeconds = song.LengthSeconds,
					Name = song.Name,
					PublishDate = song.PublishDate,
					RatingScore = song.RatingScore,
					SongType = song.SongType,
					Album = song.Albums.FirstOrDefault(),
				}).ToArray(),
				Term = songs.Term,
				TotalCount = songs.TotalCount,
			};
		}

		public AlbumDetailsContract GetAlbumById(int id)
		{
			var q = new Dictionary<string, object>
			{
				{ "fields", string.Join(",", new[] { AlbumOptionalFields.AdditionalNames }) },
			};
			var album = CallClient<AlbumForApiContract>($"{_endpoint}/albums/{id}?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new AlbumDetailsContract
			{
				AdditionalNames = album.AdditionalNames,
				ArtistString = album.ArtistString,
				DiscType = album.DiscType,
				Id = album.Id,
				Name = album.Name,
				ReleaseDate = album.ReleaseDate,
			};
		}

		public AlbumContract GetAlbumDetails(string term, AlbumSortRule sort = AlbumSortRule.NameThenReleaseDate) => FindAlbums(term, maxResults: 10, sort: sort).Items.FirstOrDefault();

		public ArtistDetailsContract GetArtistById(int id)
		{
			var q = new Dictionary<string, object>
			{
				{ "fields", string.Join(",", new[] { ArtistOptionalFields.AdditionalNames }) },
			};
			var artist = CallClient<ArtistForApiContract>($"{_endpoint}/artists/{id}?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new ArtistDetailsContract
			{
				AdditionalNames = artist.AdditionalNames,
				ArtistType = artist.ArtistType,
				Id = artist.Id,
				Name = artist.Name,
			};
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
			var q = new Dictionary<string, object>
			{
				{ "lang", language },
				{ "fields", string.Join(",", new[] { SongOptionalFields.AdditionalNames }) },
			};
			var song = CallClient<SongForApiContract>($"{_endpoint}/songs/{id}?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new SongDetailsContract
			{
				Song = new SongContract
				{
					AdditionalNames = song.AdditionalNames,
					ArtistString = song.ArtistString,
					Id = song.Id,
					LengthSeconds = song.LengthSeconds,
					Name = song.Name,
					PublishDate = song.PublishDate,
					RatingScore = song.RatingScore,
					SongType = song.SongType,
				},
			};
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
			var q = new Dictionary<string, object>
			{
				{ "fields", string.Join(",", new[] { TagOptionalFields.AdditionalNames }) },
			};
			var tag = CallClient<TagForApiContract>($"{_endpoint}/tags/{id}?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new TagContract
			{
				AdditionalNames = tag.AdditionalNames,
				CategoryName = tag.CategoryName,
				Name = tag.Name,
				Description = tag.Description,
			};
		}

		public TagContract GetTagByName(string name)
		{
			var q = new Dictionary<string, object>
			{
				{ "fields", string.Join(",", new[] { TagOptionalFields.AdditionalNames }) },
			};
			var tag = CallClient<TagForApiContract>($"{_endpoint}/tags/byName/{name}?{string.Join("&", q.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value.ToString())}"))}");
			return new TagContract
			{
				AdditionalNames = tag.AdditionalNames,
				CategoryName = tag.CategoryName,
				Name = tag.Name,
				Description = tag.Description,
			};
		}

		public UserContract GetUser(string name, string accessKey)
		{
			throw new NotImplementedException();
		}
	}
}
