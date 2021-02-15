using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Tags
{
	public class TagContract : TagBaseContract
	{
		[DataMember]
		public string Description { get; set; }
	}
}
