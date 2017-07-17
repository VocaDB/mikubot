using System;
using MikuBot.Commands;

namespace MikuBot {

	public class BotCommand {

		public BotCommand(MsgCommand cmd, IBotContext bot) {

			CmdReader cmdReader = null;
			Command = CommandString = string.Empty;

			if (cmd.Receiver.Equals(bot.OwnNick)) {

				Method = BotCommandMethod.Private;				

				cmdReader = new CmdReader(cmd.Text);
				var cmdStringReader = new CmdReader(cmd.Text);
				cmdStringReader.ReadNext();
				CommandString = cmdStringReader.ReadToEnd();

			} else if (cmd.Text.ToLowerInvariant().StartsWith(bot.OwnNick.LowercaseName + ":")) {

				Method = BotCommandMethod.Highlight;

				int highlightPos = cmd.Text.IndexOf(':');

				cmdReader = new CmdReader(cmd.Text, highlightPos + 1);
				var cmdStringReader = new CmdReader(cmd.Text, highlightPos + 1);
				cmdStringReader.ReadNext();
				CommandString = cmdStringReader.ReadToEnd();
			
			} else if (!string.IsNullOrEmpty(bot.HighlightShortcut) && cmd.Text.StartsWith(bot.HighlightShortcut)) {

				Method = BotCommandMethod.Highlight;

				cmdReader = new CmdReader(cmd.Text, bot.HighlightShortcut.Length);
				var cmdStringReader = new CmdReader(cmd.Text, bot.HighlightShortcut.Length);
				cmdStringReader.ReadNext();
				CommandString = cmdStringReader.ReadToEnd();

			}

			if (cmdReader != null) {

				Command = cmdReader.ReadNext();
				Params = new ParamCollection(cmdReader);
				
			} else {

				Params = new ParamCollection();

			}

		}

		/// <summary>
		/// Command. Cannot be null.
		/// </summary>
		public string Command { get; private set; }

		/// <summary>
		/// Command method.
		/// </summary>
		public BotCommandMethod Method { get; private set; }

		/// <summary>
		/// Parameter collection. Cannot be null.
		/// </summary>
		public ParamCollection Params { get; private set; }

		/// <summary>
		/// Full command string including command and parameters. Cannot be null.
		/// </summary>
		public string CommandString { get; private set; }

		public bool Is(string cmdString) {
			return (Command.Equals(cmdString, StringComparison.InvariantCultureIgnoreCase));
		}

		public bool Is(string cmdString, BotCommandMethod method) {
			return (Is(cmdString) && Method == method);
		}

	}
	
	public enum BotCommandMethod {
		
		/// <summary>
		/// No bot command.
		/// </summary>
		Nothing,

		/// <summary>
		/// Private message.
		/// </summary>
		Private,

		/// <summary>
		/// Highlight.
		/// </summary>
		Highlight

	}

}
