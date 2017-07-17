using System.Collections.Generic;
using System.Linq;

namespace MikuBot {

	public class ChannelStatus {

		private readonly UserCollection users = new UserCollection();

		public ChannelStatus(IrcName name) {
			Name = name;
		}

		public IrcName Name { get; private set; }

		public IReadOnlyUserCollection Users {
			get { return users; }
		}

		public bool AddUser(Hostmask user) {

			return users.Add(user);

		}

		public bool ChangeUserNick(Hostmask userHost, IrcName newNick) {

			if (!userHost.IsValid)
				return false;

			RemoveUser(userHost);

			AddUser(new Hostmask(newNick, userHost.Ident, userHost.Hostname));
			return true;

		}

		public Hostmask GetHostmaskByNick(IrcName nick) {

			return users.GetHostmaskByNick(nick);

		}

		public bool RemoveUser(Hostmask user) {

			return users.Remove(user);

		}

		public bool RemoveUserByNick(IrcName nick) {

			return RemoveUser(GetHostmaskByNick(nick));

		}

	}

}
