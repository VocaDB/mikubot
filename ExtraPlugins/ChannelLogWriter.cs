﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class ChannelWriter : GenericModuleBase
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ChannelWriter));

		private string logFile;

		private void WriteLine(string line)
		{
			try
			{
				using (var writer = new StreamWriter(logFile, true))
				{
					writer.WriteLine(line);
				}
			}
			catch (IOException x)
			{
				log.Warn("Unable to write log line", x);
			}
		}

		public override string HelpText
		{
			get { return "Logs all received messages in raw format, as they arrive from the server."; }
		}

		public override string Name
		{
			get { return "ChannelWriter"; }
		}

		public override InitialModuleStatus InitialStatus
		{
			get { return InitialModuleStatus.Disabled; }
		}

		public override void OnMessageReceived(string line, IBotContext bot)
		{
			WriteLine("[" + DateTime.Now + "] " + line);
		}

		public override void OnMessageSent(string line, IBotContext bot)
		{
			WriteLine(string.Format("[{0}] :{1}!SELF {2}", DateTime.Now, bot.OwnNick, line));
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			logFile = bot.Config.LogFile;
		}
	}
}
