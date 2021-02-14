using System.Runtime.Serialization;
using VocaDb.Model.Domain.Users;

namespace VocaDb.Model.DataContracts.Users
{
	public class UserContract
	{
		[DataMember]
		public UserGroupId GroupId { get; set; }
	}
}
