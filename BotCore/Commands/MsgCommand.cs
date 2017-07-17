namespace MikuBot.Commands {

	/// <summary>
	/// PRIVMSG command
	/// </summary>
	public class MsgCommand : IrcCommand {

		public const string MessageName = "PRIVMSG";

		//private readonly bool highlightMe;
		private readonly BotCommand botCommand;
		private readonly IrcName receiver;
		private readonly UserData sender;

		public MsgCommand(string prefix, ParamCollection paramCollection, IBotContext bot)
			: base(prefix, MessageName, paramCollection) {

			sender = new UserData(SenderHost, bot);
			receiver = new IrcName(ReceiverParam);
			botCommand = new BotCommand(this, bot);

		}

		/// <summary>
		/// Bot command. Cannot be null.
		/// </summary>
		public BotCommand BotCommand {
			get { return botCommand; }
		}

		/// <summary>
		/// Returns either the channel this message was sent to, or if the message was sent as a PM, the nick sender of that message.
		/// </summary>
		/// <remarks>
		/// This is useful for replying to commands: if the message was sent via channel, also the reply will be posted there.
		/// </remarks>
		public override IrcName ChannelOrSenderNick {
			get {
				return ReceiverIsChannel ? Receiver : Sender.Nick;
			}
		}

		public bool HasBotCommand {
			get {
				return (botCommand.Method != BotCommandMethod.Nothing);
			}
		}

		/// <summary>
		/// Whether this chat message higlights the bot.
		/// </summary>
		public bool HighlightMe {
			get {
				return (botCommand.Method == BotCommandMethod.Highlight);
			}
		}

		public IrcName Receiver {
			get {
				return receiver;
			}
		}

		public string ReceiverParam {
			get {
				return ParamCollection.ParamOrEmpty(0);
			}			
		}

		public bool ReceiverIsChannel {
			get {
				return Receiver.IsChannel;
			}
		}

		public Hostmask SenderHost {
			get {
				return new Hostmask(Prefix);
			}
		}

		/// <summary>
		/// Sender of this message. Cannot be null.
		/// </summary>
		public override UserData Sender {
			get {
				return sender;
			}
		}

		/// <summary>
		/// Message text
		/// </summary>
		public string Text { get {
			return ParamCollection.Trailing;
		}}

		public Receiver Reply(IrcWriter writer) {
			return new Receiver(writer, ChannelOrSenderNick);
		}

	}

}
