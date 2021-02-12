using System.Linq;
using Google.Apis.YouTube.v3.Data;

namespace MikuBot.ExtraPlugins.Helpers
{
	public static class YoutubeUtils
	{
		/*public static YouTubeRequest YouTubeRequest {
			get {
				var settings = new YouTubeRequestSettings("MikuBot", null);
				return new YouTubeRequest(settings);
			}
		}*/

		public static string Format(Video video)
		{
			//var date = (video.YouTubeEntry != null ? video.YouTubeEntry.Published : video.Updated);
			//var date = video.FileDetails.CreationTime;
			var title = video.Snippet.Title;
			var author = video.Snippet.ChannelTitle;
			var date = video.Snippet.PublishedAt;

			var viewCount = video.Statistics.ViewCount;
			return string.Format("{0}{1}{0} by {2} at {3}, {4} views", Formatting.Bold, title, author, date, viewCount);
		}
	}
}
