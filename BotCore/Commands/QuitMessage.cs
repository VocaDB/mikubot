namespace MikuBot.Commands
{
	/// <summary>
	/// QUIT message
	/// </summary>
	public class QuitMessage : IrcCommand
	{
		public const string MessageName = "QUIT";

		private readonly UserData sender;

		public QuitMessage(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, MessageName, paramCollection)
		{
			sender = new UserData(SenderHost, bot);
		}

		public string Comment
		{
			get
			{
				return ParamCollection.Trailing;
			}
		}

		public UserData QuitUser
		{
			get { return Sender; }
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
