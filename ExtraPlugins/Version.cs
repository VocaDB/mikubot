using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	public class Version : MsgCommandModuleBase {

		public override int CooldownChannelMs {
			get { return 10000; }
		}

		public override int CooldownUserMs {
			get { return 30000; }
		}

		public override string CommandDescription {
			get { return "Displays bot version."; }
		}

		public override string Name {
			get { return "Version"; }
		}


		public override void HandleCommand(MsgCommand chat, IBotContext bot) {

			if (!CheckCall(chat, bot))
				return;

			var msg = string.Format("MikuBot v{0}", Application.ProductVersion);

			bot.Writer.Msg(chat.ChannelOrSenderNick, msg);

		}

	}

}
