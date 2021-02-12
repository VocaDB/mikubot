namespace MikuBot.Commands
{
	/// <summary>
	/// KICK command
	/// </summary>
	public class KickCommand : IrcCommand
	{
		private readonly bool kickedUserIsSelf;
		private readonly UserData sender;

		public KickCommand(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, "KICK", paramCollection)
		{
			kickedUserIsSelf = BotHelper.IsOwnNick(KickedUserNick, bot);
			sender = new UserData(SenderHost, bot);
		}

		public IrcName Channel
		{
			get
			{
				return new IrcName(ParamCollection.ParamOrEmpty(0));
			}
		}

		public override IrcName ChannelOrSenderNick
		{
			get
			{
				return Channel;
			}
		}

		public string Comment
		{
			get
			{
				return ParamCollection.Trailing;
			}
		}

		public bool KickedUserIsSelf
		{
			get { return kickedUserIsSelf; }
		}

		public IrcName KickedUserNick
		{
			get
			{
				return new IrcName(ParamCollection.ParamOrEmpty(1));
			}
		}

		public override UserData Sender
		{
			get { return sender; }
		}

		public Hostmask SenderHost
		{
			get
			{
				return new Hostmask(Prefix);
			}
		}
	}
}
