using System;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class DateAndTime : MsgCommandModuleBase {

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override int CooldownUserMs {
			get { return 30000; }
		}

		public override string CommandDescription {
			get { return "Displays the current system date and time."; }
		}

		public override string Name {
			get { return "DateTime"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var d = DateTime.Now;

			bot.Writer.Msg(msg.ChannelOrSenderNick, string.Format("Current date and time is {0} {1} (GMT+{2})", 
				d.ToString("g"), TimeZone.CurrentTimeZone.StandardName, TimeZone.CurrentTimeZone.GetUtcOffset(d).ToString("hh\\:mm")));

		}

	}
}
