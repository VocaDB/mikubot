using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Songs
{
	public class LyricsForSongContract
	{
		[DataMember(EmitDefaultValue = false)]
		public string Value { get; set; }
	}
}
