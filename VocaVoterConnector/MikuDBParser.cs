using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using MikuBot.VocaDBConnector;
using MikuBot.VocaDBConnector.Helpers;

namespace MikuBot.VocaVoterConnector {

	public class MikuDBParser : MsgCommandModuleBase {

		private VocaVoterConnectorFile connectorFile;

		private readonly Regex[] linkMatchers = new[] {
			new Regex(@"(mikudb.com/[0-9]+/\w+)"),
		};

		private void GetEntryInfo(Receiver receiver, string url) {

			var album = connectorFile.VocaDbClient.GetAlbumByLinkUrl(url);

			if (album == null) {
				return;
			}

			receiver.Msg(EntryFormattingHelper.FormatAlbumWithUrl(album, connectorFile.Config));

		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			connectorFile = (VocaVoterConnectorFile)moduleFile;

		}

		public override string HelpText {
			get { return "Parses MikuDB links. By prefixing the link with 'nolink', all link parsing is skipped. This is useful if you don't want some link to be parsed."; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "MikuDBParser"; }
		}

		public override string UsageHelp {
			get { return "[<nolink>] <MikuDB URL>"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (cmd.BotCommand.Is("NoLink"))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);

			var matcher = linkMatchers.FirstOrDefault(m => m.IsMatch(cmd.Text));

			if (matcher == null)
				return;

			var match = matcher.Match(cmd.Text);
			var url = match.Groups[1].Value;

			Task.Factory.StartNew(() => GetEntryInfo(receiver, url))
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);

		}

	}

}
