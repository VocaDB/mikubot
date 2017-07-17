using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.DbPlugins {

	public class Kivid : MsgCommandModuleBase {

		private string latestLink;
		private DbPluginsModuleFile modules;

		private void CheckLink(Receiver receiver, IrcName chan, string uri) {

			var record = modules.CommonServices.FindLinkRecord(uri, chan);

			if (record == null || (record.Nick.Name != "kyllakivi" && record.Nick.Name != "kivi")) {
				receiver.Msg("No");
			} else {
				receiver.Msg("Yes");
			}

		}

		public override string UsageHelp {
			get { return "kivi'd [<url>] [<channel>]"; }
		}

		public override string CommandDescription {
			get { return "Checks if a link is kivi'd."; }
		}

		public override bool IsPassive {
			get { return false; }
		}

		public override string Name {
			get { return "Kivi'd"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!cmd.BotCommand.Is(Name)) {

				var text = cmd.Text.ToLowerInvariant();

				var uris = BotHelper.GetUris(text).Select(u => u.AbsoluteUri).ToArray();

				if (uris.Any())
					latestLink = uris.First();

			} else {

				var uri = cmd.BotCommand.Params.ParamOrNull(0) ?? latestLink;
				var chan = cmd.BotCommand.Params.HasParam(1) ? new IrcName(cmd.BotCommand.Params[1]) : cmd.ChannelOrSenderNick;

				var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

				if (string.IsNullOrEmpty(uri)) {
					receiver.Msg("No link specified.");
					return;
				}

				CheckLink(receiver, chan, uri);

			}

			//Task.Factory.StartNew(() => RecordLinks(uris, cmd.Sender.Nick))
			//	.ContinueWith(BotHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);	

		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {
			modules = (DbPluginsModuleFile)moduleFile;
		}

	}

}
