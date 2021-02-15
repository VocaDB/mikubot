using System.Runtime.Serialization;
using VocaDb.Model.DataContracts.Users;

namespace VocaDb.Model.DataContracts.Songs
{
	public class SongListContract : SongListBaseContract
	{
		[DataMember]
		public UserForApiContract Author { get; set; }
	}
}
