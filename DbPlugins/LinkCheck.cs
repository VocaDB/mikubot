using System;
using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.DbPlugins {

	public class LinkCheck : MsgCommandModuleBase {

		private string latestLink;
		private DbPluginsModuleFile modules;

		private string CropLink(string text) {

			return text.Length < 45 ? text : text.Substring(0, 42) + "...";

		}

		private void CheckLink(Receiver receiver, IrcName chan, string uri) {

			var record = modules.CommonServices.FindLinkRecord(uri, chan);

			if (record == null) {
				receiver.Msg("No record of link '" + CropLink(uri) + "'");
			} else {

				var matchType = record.Url.Equals(uri, StringComparison.InvariantCultureIgnoreCase) ? "Match" : "Partial match";
				var url = CropLink(record.Url);

				receiver.Msg(string.Format("{0}: Link '{1}' was first posted by {2} at {3}", matchType, url, record.Nick, record.Date));

			}

		}

		public override string UsageHelp {
			get { return "link [<url>] [<channel>]"; }
		}

		public override string CommandDescription {
			get { return "Checks when a specified link was first posted. If no link is specified, the latest posted link is checked."; }
		}

		public override bool IsPassive {
			get { return false; }
		}

		public override string Name {
			get { return "Link"; }
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
