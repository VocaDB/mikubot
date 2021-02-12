namespace MikuBot.Commands
{
	/// <summary>
	/// JOIN command
	/// </summary>
	/// <remarks>
	/// This message is received when this bot joins a channel or when another user joins a channel where this bot is on.
	/// </remarks>
	public class JoinCommand : IrcCommand
	{
		public const string MessageName = "JOIN";

		private readonly UserData sender;

		public JoinCommand(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, MessageName, paramCollection)
		{
			sender = new UserData(SenderHost, bot);
		}

		public IrcName Channel
		{
			get
			{
				return new IrcName(ParamCollection.Trailing);
			}
		}

		public override IrcName ChannelOrSenderNick
		{
			get { return Channel; }
		}

		public UserData JoinedUser
		{
			get { return sender; }
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
