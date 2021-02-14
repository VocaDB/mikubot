using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Tags
{
	public class TagForApiContract
	{
		/// <summary>
		/// Comma-separated list of all other names that aren't the display name.
		/// </summary>
		[DataMember(EmitDefaultValue = false)]
		public string AdditionalNames { get; set; }

		[DataMember]
		public string CategoryName { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember]
		public string Name { get; set; }

	}
}
