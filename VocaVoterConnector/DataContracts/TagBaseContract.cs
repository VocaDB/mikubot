using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Tags
{
	public class TagBaseContract
	{
		/// <summary>
		/// Additional names - optional field.
		/// </summary>
		[DataMember(EmitDefaultValue = false)]
		public string AdditionalNames { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string CategoryName { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}
