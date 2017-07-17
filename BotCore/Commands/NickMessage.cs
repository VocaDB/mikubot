namespace MikuBot.Commands {

	/// <summary>
	/// NICK message
	/// </summary>
	public class NickMessage : IrcCommand {

		private readonly UserData sender;

		public NickMessage(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, "NICK", paramCollection) {

			sender = new UserData(SenderHost, bot);

		}

		public IrcName NewNick {
			get {
				return new IrcName(ParamCollection.ParamOrNull(0) ?? ParamCollection.Trailing);
			}
		}

		/// <summary>
		/// Original nickname of the user who changed his nick. Can be empty.
		/// </summary>
		public IrcName OriginalNick {
			get { return Sender.Nick; }
		}

		public override UserData Sender {
			get { return sender; }
		}

		public Hostmask SenderHost {
			get {
				// Note: according to RFC, prefix should actually be nick, not hostname (?)
				return new Hostmask(Prefix);
			}
		}

	}

}
