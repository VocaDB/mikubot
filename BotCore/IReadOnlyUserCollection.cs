using System.Collections.Generic;

namespace MikuBot
{
	public interface IReadOnlyUserCollection : IEnumerable<Hostmask>
	{
		Hostmask FindUser(string query);

		Hostmask GetHostmaskByNick(IrcName nick);
	}
}
