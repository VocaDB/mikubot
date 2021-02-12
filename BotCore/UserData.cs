namespace MikuBot
{
	/// <summary>
	/// Contains basic information of an IRC user, determined using its hostname.
	/// </summary>
	public class UserData
	{
		public UserData(Hostmask host, IBotContext bot)
		{
			ParamIs.NotNull(() => bot);

			Host = host;
			IsAuthenticated = bot.Authenticator.IsAuthenticated(host);
			IsSelf = BotHelper.IsOwnNick(Nick, bot);
			UserLevel = bot.Authenticator.GetUserLevel(host);
		}

		public Hostmask Host { get; private set; }

		public bool IsAuthenticated { get; private set; }

		public bool IsSelf { get; private set; }

		public IrcName Nick
		{
			get
			{
				return Host.Nick;
			}
		}

		public BotUserLevel UserLevel { get; private set; }
	}
}
