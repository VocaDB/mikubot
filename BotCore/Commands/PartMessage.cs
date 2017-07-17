namespace MikuBot.Commands {

	/// <summary>
	/// PART message
	/// </summary>
	public class PartMessage : IrcCommand {

		public const string MessageName = "PART";

		private readonly UserData sender;

		public PartMessage(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, MessageName, paramCollection) {

			sender = new UserData(SenderHost, bot);

		}

		public IrcName Channel {
			get {
				return new IrcName(ParamCollection.ParamOrEmpty(0));
			}
		}

		public override IrcName ChannelOrSenderNick {
			get {
				return Channel;
			}
		}

		public string Comment {
			get {
				return ParamCollection.Trailing;
			}
		}

		public UserData PartedUser {
			get { return Sender; }
		}

		public override UserData Sender {
			get { return sender; }
		}

		public Hostmask SenderHost {
			get {
				return new Hostmask(Prefix);
			}
		}

	}

}
