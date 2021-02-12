using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class DnsResolve : MsgCommandModuleBase
	{
		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override string CommandDescription
		{
			get { return "Resolves DNS address"; }
		}

		public override string Name
		{
			get { return "Dns"; }
		}

		public override string UsageHelp
		{
			get { return "dns <url>"; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			if (!CheckCall(chat, bot))
				return;

			var reply = chat.Reply(bot.Writer);
			var param = chat.BotCommand.Params[0];
			string host;

			try
			{
				var u = new Uri(param);
				host = u.DnsSafeHost;
			}
			catch (UriFormatException)
			{
				host = param;
			}
			IPAddress[] addresses;

			try
			{
				addresses = Dns.GetHostAddresses(host);
			}
			catch (ArgumentException x)
			{
				reply.Msg("DNS (error): " + x.Message);
				return;
			}
			catch (SocketException x)
			{
				reply.Msg("DNS (error): " + x.Message);
				return;
			}

			if (!addresses.Any())
			{
				reply.Msg("DNS: Nothing found.");
				return;
			}

			var addressStr = addresses.Select(a => a.ToString());
			reply.Msg("Addresses for " + host + ": " + string.Join(", ", addressStr));
		}
	}
}
