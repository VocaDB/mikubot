using System.Collections.Generic;

namespace MikuBot.Security
{
	public interface IAuthenticator
	{
		bool Authenticate(Hostmask client, string key);

		void Authenticate(Hostmask client, string key, BotUserLevel userLevel);

		IDictionary<Hostmask, AuthKey> AuthClients { get; }

		BotUserLevel GetUserLevel(Hostmask client);

		bool IsAuthenticated(Hostmask client);

		bool KeyMatch(string key);

		bool Unauthenticate(Hostmask client);
	}
}
