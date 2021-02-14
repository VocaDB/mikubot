using System.Runtime.Serialization;
using VocaDb.Model.Domain.Albums;
using VocaDb.Model.Domain.Artists;
using VocaDb.Model.Domain.Songs;

namespace VocaDb.Model.DataContracts.Api
{
	public class EntryForApiContract
	{
		[DataMember(EmitDefaultValue = false)]
		public string AdditionalNames { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public ArtistType? ArtistType { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DiscType? DiscType { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public SongType? SongType { get; set; }
	}
}
