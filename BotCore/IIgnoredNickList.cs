using System;
using System.Collections.Generic;

namespace MikuBot
{
	public interface IIgnoredNickList : IEnumerable<Hostmask>
	{
		void Ignore(Hostmask user, DateTime? endTime = null);

		bool IsIgnored(Hostmask user);

		bool Unignore(Hostmask user);
	}
}
