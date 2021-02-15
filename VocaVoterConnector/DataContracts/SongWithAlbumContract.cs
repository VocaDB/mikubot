using System.Runtime.Serialization;
using VocaDb.Model.DataContracts.Albums;

namespace VocaDb.Model.DataContracts.Songs
{
	public class SongWithAlbumContract : SongContract
	{
		[DataMember]
		public AlbumContract Album { get; set; }
	}
}
