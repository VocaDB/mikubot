using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts
{
	public class OptionalDateTimeContract
	{
		public OptionalDateTimeContract() { }

		[DataMember]
		public int? Day { get; set; }

		[DataMember]
		public string Formatted { get; set; }

		[DataMember]
		public bool IsEmpty { get; set; }

		[DataMember]
		public int? Month { get; set; }

		[DataMember]
		public int? Year { get; set; }
	}
}
