using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MikuBot {

	public class UserCollection : IReadOnlyUserCollection {

		private readonly HashSet<Hostmask> users = new HashSet<Hostmask>();

		public bool Add(Hostmask user) {

			return users.Add(user);

		}

		public bool ChangeNick(IrcName oldNick, IrcName newNick) {

			var userHost = GetHostmaskByNick(oldNick);
			RemoveByNick(oldNick);

			if (userHost.IsValid) {
				Add(new Hostmask(newNick, userHost.Ident, userHost.Hostname));
				return true;
			}
			return false;

		}

		public void Clear() {
			users.Clear();
		}

		public Hostmask FindUser(string query) {

			// TODO: support wildcards

			if (users.Any(u => u.Equals(query)))
				return users.First(u => u.Equals(query));

			if (users.Any(u => u.Nick.Equals(query)))
				return users.First(u => u.Nick.Equals(query));

			return Hostmask.Empty;

		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<Hostmask> GetEnumerator() {
			return users.GetEnumerator();
		}

		public Hostmask GetHostmaskByNick(IrcName nick) {

			if (users.Any(u => u.Nick == nick))
				return users.First(u => u.Nick == nick);
			else
				return Hostmask.Empty;

		}

		public bool Remove(Hostmask user) {

			return users.Remove(user);

		}

		public bool RemoveByNick(IrcName nick) {

			return Remove(GetHostmaskByNick(nick));

		}

	}

}
