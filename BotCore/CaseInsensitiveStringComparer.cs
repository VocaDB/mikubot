using System;
using System.Collections.Generic;

namespace MikuBot {

	public class CaseInsensitiveStringComparer : IEqualityComparer<string> {

		public bool Equals(string x, string y) {
			return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(string obj) {
			return obj.ToLowerInvariant().GetHashCode();
		}

	}

}
