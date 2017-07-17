using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using NicoApi;

namespace MikuBot.ExtraPlugins {

	public class NicoParser : MsgCommandModuleBase {

		public static string GetNodeTextOrEmpty(XElement node) {

			if (node == null)
				return string.Empty;

			return node.Value;

		}

		public static string GetNodeTextOrEmpty(XDocument doc, string xpath) {

			return GetNodeTextOrEmpty(doc.XPathSelectElement(xpath));

		}

		private void GetVideoData(Receiver receiver, string id) {

			VideoDataResult data;

			try {
				data = VideoApiClient.GetVideoData(id, true);
			} catch (NicoApiException x) {
				receiver.Msg("NicoVideo (error): " + x.Message);
				return;				
			}

			receiver.Msg(string.Format("NicoVideo: {0}{1}{0} at {2} by {3}, {4} views", 
				Formatting.Bold, data.Title, data.Created, data.Author, data.Views));

		}

		public override string HelpText {
			get { return "Parses NicoNicoDouga links"; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "Nicovideo"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (cmd.BotCommand.Is("NoLink"))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var possibleUrl = cmd.Text;
			var id = NicoUrlHelper.GetIdByUrl(possibleUrl);

			if (string.IsNullOrEmpty(id) || id.StartsWith("lv") || id.StartsWith("nw"))
				return;

			//GetPageContent(receiver, url);							// Synchronized version
			Task.Factory.StartNew(() => GetVideoData(receiver, id))	// Async version
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);	

		}

	}

}
