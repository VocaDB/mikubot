using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuBot.Modules;
using MikuBot.Commands;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MikuBot.ExtraPlugins
{
	public class Whois : MsgCommandModuleBase
	{
		public override string Name
		{
			get { return "Whois"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!cmd.BotCommand.Is(Name) || !cmd.BotCommand.Params.HasParam(0))
				return;

			var url = cmd.BotCommand.Params[0];

			using (var tcp = new TcpClient("whois.joker.com", 43))
			{
				Byte[] arrDomain = Encoding.ASCII.GetBytes(url.ToCharArray());

				var s = tcp.GetStream();
				using (var writer = new StreamWriter(s))
				{
					writer.WriteLine(url);
					writer.Flush();
				}

				var sr = new StreamReader(s);
				string strLine = null;

				bool output = false;
				var receiver = cmd.Reply(bot.Writer);

				while (null != (strLine = sr.ReadLine()))
				{
					//if (output)
					receiver.Notice(strLine);

					/*if (!output) {
						output = true;
					}*/
				}
			}
		}

		public override InitialModuleStatus InitialStatus
		{
			// TODO: remove when finished
			get { return InitialModuleStatus.NotLoaded; }
		}
	}
}
