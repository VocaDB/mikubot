namespace MikuBot.Commands
{
	/// <summary>
	/// INVITE command
	/// </summary>
	public class InviteCommand : IrcCommand
	{
		private readonly UserData sender;

		public InviteCommand(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, "INVITE", paramCollection)
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

		public IrcName ReceiverNick
		{
			get
			{
				return new IrcName(ParamCollection.ParamOrEmpty(0));
			}
		}

		public override UserData Sender
		{
			get
			{
				return sender;
			}
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
