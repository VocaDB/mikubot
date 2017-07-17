using MikuBot.Commands.Numeric;

namespace MikuBot.Commands {

	/// <summary>
	/// Base class for IRC commands
	/// </summary>
	public class IrcCommand {

		private static NumericReply CreateNumericReply(string prefix, string command, ParamCollection paramCollection, int replyCode) {
			
			switch (replyCode) {
				case (int)ReplyCode.RPL_WHOREPLY:
					return new WhoReply(prefix, command, paramCollection);
				default:
					return new NumericReply(prefix, command, paramCollection, replyCode);
			}

		}

		public static IrcCommand Parse(string cmd, IBotContext bot) {

			if (string.IsNullOrEmpty(cmd))
				return null;

			// new Regex(@"(:(?<prefix>\w+)[ ]+)?(?<command>\w+)([ ]+(?<params>\w+))*([ ]+:(?<trailing>\w+))?"); // almost works too

			var cmdReader = new CmdReader(cmd);

			bool hasPrefix = cmdReader.Peek == ':';

			string prefix = (hasPrefix ? cmdReader.ReadNext().Substring(1) : null);
			string command = cmdReader.ReadNext().ToUpperInvariant();

			var paramCollection = new ParamCollection(cmdReader);

			switch (command) {
				case "INVITE":
					return new InviteCommand(prefix, paramCollection, bot);
				case "JOIN":
					return new JoinCommand(prefix, paramCollection, bot);
				case "KICK":
					return new KickCommand(prefix, paramCollection, bot);
				case "KILL":
					return new KillMessage(prefix, paramCollection);
				case "NICK":
					return new NickMessage(prefix, paramCollection, bot);
				case "PART":
					return new PartMessage(prefix, paramCollection, bot);
				case "PING":
					return new PingCommand(prefix, paramCollection);
				case "PRIVMSG":
					return new MsgCommand(prefix, paramCollection, bot);
				case "QUIT":
					return new QuitMessage(prefix, paramCollection, bot);
				default:
					int replyCode;
					if (int.TryParse(command, out replyCode))
						return CreateNumericReply(prefix, command, paramCollection, replyCode);
					else
						return new IrcCommand(prefix, command, paramCollection);
			}

		}

		private IrcCommand() {
			Command = Prefix = string.Empty;
		}

		public IrcCommand(string prefix, string command, ParamCollection paramCollection)
			: this() {

			Prefix = prefix ?? Prefix;
			Command = command;
			ParamCollection = paramCollection;

		}

		/// <summary>
		/// If this message was sent on a channel, name of that channel, or if the message was sent to this bot directly, the name of the sender.
		/// This name can be used for replying to the sender.
		/// </summary>
		public virtual IrcName ChannelOrSenderNick {
			get {
				return (Sender != null ? Sender.Nick : IrcName.Empty);
			}
		}

		public string Command { get; set; }

		public ParamCollection ParamCollection { get; private set; }

		/// <summary>
		/// Command prefix. Cannot be null. Can be empty.
		/// </summary>
		public string Prefix { get; set; }

		/// <summary>
		/// Sender of the message, if known. Can be null.
		/// </summary>
		public virtual UserData Sender {
			get {
				return null;
			}
		}

	}

}
