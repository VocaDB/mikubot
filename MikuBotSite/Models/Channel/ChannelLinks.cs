using MikuBot.DbModel.DataContracts;

namespace MikuBot.Site.Models.Channel {

	public class ChannelLinks {

		public ChannelLinks(string channel, string nick, LinkRecordContract[] links, int page) {

			Channel = channel;
			Links = links;
			Nick = nick;
			Page = page;

		}

		public string Channel { get; set; }

		public LinkRecordContract[] Links { get; set; }

		public string Nick { get; set; }

		public int Page { get; set; }

	}

}