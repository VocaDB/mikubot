using System;
using System.Linq;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class LogTail : MsgCommandModuleBase
	{
		private MessageBuffer messageBuffer;

		private int IntParamOrDefault(ParamCollection col, int index, int def)
		{
			if (!col.HasParam(index))
				return def;

			int val;
			if (int.TryParse(col[index], out val))
				return val;
			else
				return def;
		}


		public override string CommandDescription
		{
			get
			{
				return "Displays the last log lines.";
			}
		}

		public override string Name
		{
			get { return "LogTail"; }
		}

		public override string UsageHelp
		{
			get
			{
				return "logtail [<number of lines>] [<channel name>]";
			}
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot)
		{
			if (!msg.BotCommand.Is(Name))
				return;

			var lineCount = Math.Min(IntParamOrDefault(msg.BotCommand.Params, 0, 10), 20);
			var channel = msg.BotCommand.Params.HasParam(1) ? new IrcName(msg.BotCommand.Params[1]) : msg.ChannelOrSenderNick;

			if (!channel.IsChannel)
			{
				bot.Writer.Msg(msg.ChannelOrSenderNick, "Channel must be specified.");
				return;
			}

			var lines = messageBuffer.GetLines(channel, lineCount).ToArray();
			var receiver = new Receiver(bot.Writer, msg.Sender.Nick);

			if (!lines.Any())
				receiver.Msg("No lines.");

			foreach (var line in lines)
			{
				receiver.Msg(string.Format("[{0}] <{1}> {2}", line.Timestamp.ToShortTimeString(), line.Message.Sender.Nick, line.Message.Text));
			}

			//File.ReadLines(logFile).Reverse().Take()

			/*using (var reader = new StreamReader(logFile)) {
				reader.BaseStream.Seek()
			}*/
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			if (moduleFile == null)
				return;

			messageBuffer = ((ExtraPluginsModuleFile)moduleFile).MessageBuffer;
		}
	}
}
