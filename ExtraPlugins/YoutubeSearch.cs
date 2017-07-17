using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins {

	/*public class YoutubeSearch : MsgCommandModuleBase {

		private void Search(Receiver receiver, string queryString) {

			var request = YoutubeUtils.YouTubeRequest;
			var query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
			query.OrderBy = "relevance";
			query.Query = queryString;
			query.SafeSearch = YouTubeQuery.SafeSearchValues.None;

			var videoFeed = request.Get<Video>(query);

			if (!videoFeed.Entries.Any()) {
				receiver.Msg("Youtube: no results.");
				return;
			}

			var first = videoFeed.Entries.First();

			receiver.Msg(string.Format("Youtube: http://youtu.be/{0} - {1}", first.VideoId, YoutubeUtils.Format(first)));

		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (!CheckCall(cmd, bot))
				return;

			var query = cmd.BotCommand.CommandString;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			Task.Factory.StartNew(() => Search(receiver, query))
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);

		}

		public override int CooldownChannelMs {
			get { return 3000; }
		}

		public override int CooldownUserMs {
			get { return 10000; }
		}

		public override string HelpText {
			get { return "Searches for Youtube for a video."; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "Yt"; }
		}

		public override string UsageHelp {
			get { return "yt <search query>"; }
		}
	}*/

}
