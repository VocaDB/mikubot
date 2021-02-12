using System;
using System.Collections.Generic;

namespace MikuBot
{
	public class HostmaskHostnameEqualityComparer : IEqualityComparer<Hostmask>
	{
		public bool Equals(Hostmask x, Hostmask y)
		{
			return x.Hostname.Equals(y.Hostname, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(Hostmask obj)
		{
			return obj.Hostname.ToLowerInvariant().GetHashCode();
		}
	}
}
