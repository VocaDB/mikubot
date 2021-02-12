using System;
using MikuBot.DbModel.Model;

namespace MikuBot.DbModel.DataContracts
{
	public class LinkRecordContract
	{
		public LinkRecordContract()
		{
			Title = string.Empty;
		}

		public LinkRecordContract(LinkRecord linkRecord)
			: this()
		{
			ParamIs.NotNull(() => linkRecord);

			Channel = new IrcName(linkRecord.Channel);
			Date = linkRecord.Date;
			Description = linkRecord.Description;
			Nick = new IrcName(linkRecord.Nick);
			Title = linkRecord.Title;
			Url = linkRecord.Url;
		}

		public LinkRecordContract(string url, IrcName nick, IrcName channel, string line)
			: this()
		{
			ParamIs.NotNullOrEmpty(() => url);

			Nick = nick;
			Url = url;
			Channel = channel;

			Date = DateTime.Now;
			Description = line;
		}

		public IrcName Channel { get; set; }

		public DateTime Date { get; set; }

		public string Description { get; set; }

		public IrcName Nick { get; set; }

		public string Title { get; set; }

		public string Url { get; set; }
	}
}
