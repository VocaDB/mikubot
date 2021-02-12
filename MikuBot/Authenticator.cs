using System;
using System.Collections.Generic;
using System.Linq;
using MikuBot.Security;

namespace MikuBot
{
	public class Authenticator : IAuthenticator
	{
		private readonly Dictionary<string, AuthKey> authKeys;

		private readonly Dictionary<Hostmask, AuthKey> authClients = new Dictionary<Hostmask, AuthKey>();

		public Authenticator(Config config)
		{
			ParamIs.NotNull(() => config);

			authKeys = config.AuthKeys
				.Select(k => new AuthKey(k, BotUserLevel.Admin))
				.ToDictionary(k => k.Key, k => k);
		}

		public void Authenticate(Hostmask clientHost, string key, BotUserLevel userLevel)
		{
			var authKey = new AuthKey(key, userLevel);

			if (!IsAuthenticated(clientHost))
				authClients.Add(clientHost, authKey);
			else
				authClients[clientHost] = authKey;
		}

		public bool Authenticate(Hostmask clientHost, string key)
		{
			if (!authKeys.ContainsKey(key))
				return false;

			var authKey = authKeys[key];

			if (!IsAuthenticated(clientHost))
				authClients.Add(clientHost, authKey);
			else
				authClients[clientHost] = authKey;

			return true;
		}

		public BotUserLevel GetUserLevel(Hostmask client)
		{
			return (authClients.ContainsKey(client) ? authClients[client].UserLevel : BotUserLevel.Unidentified);
		}

		public IDictionary<Hostmask, AuthKey> AuthClients
		{
			get
			{
				return authClients;
			}
		}

		public bool IsAuthenticated(Hostmask clientHost)
		{
			return authClients.ContainsKey(clientHost);
		}

		public bool KeyMatch(string key)
		{
			return authKeys.ContainsKey(key);
		}

		public bool Unauthenticate(Hostmask user)
		{
			return authClients.Remove(user);
		}
	}
}
