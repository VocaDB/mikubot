using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MikuBot
{
	public class IgnoredNickList : IIgnoredNickList
	{
		private readonly Dictionary<Hostmask, DateTime?> ignoredUsers = new Dictionary<Hostmask, DateTime?>(new HostmaskHostnameEqualityComparer());

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Hostmask> GetEnumerator()
		{
			return ignoredUsers.Keys.GetEnumerator();
		}

		public void Ignore(Hostmask user, DateTime? endTime = null)
		{
			if (!ignoredUsers.ContainsKey(user))
			{
				ignoredUsers.Add(user, endTime);
			}
			else
			{
				var entry = ignoredUsers[user];
				if (entry.HasValue && (endTime == null || (entry.Value < endTime.Value)))
					ignoredUsers[user] = endTime;
			}
		}

		public bool IsIgnored(Hostmask user)
		{
			if (!ignoredUsers.ContainsKey(user))
				return false;

			var entry = ignoredUsers[user];

			if (entry.HasValue && entry.Value < DateTime.Now)
			{
				ignoredUsers.Remove(user);
				return false;
			}

			return true;
		}

		public bool Unignore(Hostmask user)
		{
			return ignoredUsers.Remove(user);
		}
	}
}
