using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VocaDb.Model.Domain.Songs;

namespace VocaDb.Model.DataContracts.Songs
{
	public class SongListBaseContract
	{
		[DataMember]
		[JsonConverter(typeof(StringEnumConverter))]
		public SongListFeaturedCategory FeaturedCategory { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}
