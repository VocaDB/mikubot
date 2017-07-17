using System;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using PiaproClient;

namespace MikuBot.ExtraPlugins {

	public class PiaproParser : MsgCommandModuleBase {

		private void GetData(Receiver receiver, string url) {

			PostQueryResult data;

			try {
				data = new PiaproClient.PiaproClient().ParseByUrl(url);
			} catch (PiaproException x) {
				receiver.Msg(string.Format("Piapro (error): {0}", x.Message));
				return;
			}

			var lengthStr = string.Empty;
			if (data.LengthSeconds != null) {
				var length = TimeSpan.FromSeconds((double)data.LengthSeconds);
				lengthStr = string.Format(" ({0}:{1})", length.Minutes, length.Seconds);
			}

			receiver.Msg(string.Format("Piapro: {0} '{1}' by {2}{3}", data.PostType, data.Title, data.Author, lengthStr));

		}

		public override string HelpText {
			get { return "Parses Piapro links"; }
		}

		public override InitialModuleStatus InitialStatus {
			get { return InitialModuleStatus.Enabled; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "Piapro"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (cmd.BotCommand.Is("NoLink"))
				return;

			string url;
			var receiver = cmd.Reply(bot.Writer);

			if (!PiaproUrlHelper.TryGetUrl(cmd.Text, out url))
				return;

			Task.Factory.StartNew(() => GetData(receiver, url))
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);

		}

	}

}
