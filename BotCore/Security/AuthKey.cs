using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikuBot.Security
{
	public class AuthKey
	{
		public AuthKey(string key, BotUserLevel userLevel)
		{
			Key = key;
			UserLevel = userLevel;
		}

		public string Key { get; private set; }

		public BotUserLevel UserLevel { get; private set; }
	}
}
