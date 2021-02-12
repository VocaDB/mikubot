using System.Linq;

namespace MikuBot
{
	public class NickManager : INickManager
	{
		private readonly IBotContext bot;
		private int index;
		private readonly IrcName[] nicks;

		public NickManager(IBotContext bot, string[] nicks)
		{
			ParamIs.NotNull(() => bot);
			ParamIs.NotNull(() => nicks);

			if (!nicks.Any())
			{
				nicks = new[] { "MikuBot" };
			}

			this.bot = bot;
			this.nicks = nicks.Select(n => new IrcName(n)).ToArray();
			Current = Nicks[index];
		}

		public IrcName Current { get; private set; }

		public bool CurrentIsPrimary
		{
			get
			{
				return Current.Equals(Primary);
			}
		}

		public IrcName Primary
		{
			get
			{
				return Nicks.First();
			}
		}

		public IrcName Next()
		{
			index = (index + 1) % nicks.Length;

			Current = nicks[index];
			bot.Writer.Nick(Current.ToString());
			return Current;
		}

		public void Set(IrcName nick)
		{
			Current = nick;
			bot.Writer.Nick(nick.ToString());
		}

		public IrcName[] Nicks
		{
			get { return nicks; }
		}
	}
}
