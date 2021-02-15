using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Songs
{
	public class SongDetailsContract
	{
		[DataMember]
		public SongContract Song { get; set; }
	}
}
