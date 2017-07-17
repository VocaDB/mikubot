using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.VocaDBConnector {

	public class PV : MsgCommandModuleBase {

		public override string HelpText {
			get {
				return "Calls PVVocaDBAdvertiser manually if automated link parsing is disabled.";
			}
		}

		public override bool IsPassive {
			get { return false; }
		}

		public override string Name {
			get { return "PV"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var module = bot.ModuleManager.FindModule<PVVocaDBAdvertiser>();

			if (module == null)
				return;

			bot.ModuleManager.CallModule(bot, module);

		}

	}
}
