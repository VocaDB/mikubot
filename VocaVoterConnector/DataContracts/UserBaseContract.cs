using System.Runtime.Serialization;

namespace VocaDb.Model.DataContracts.Users
{
	public class UserBaseContract
	{
		[DataMember]
		public string Name { get; set; }
	}
}
