using System;
using System.Linq;
using MikuBot.DbModel.DataContracts;
using MikuBot.DbModel.Model;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;

namespace MikuBot.DbModel.Services
{
	public class CommonServices : ServiceBase
	{
		class LineEqualityComparer : IEqualityComparer<LinkRecord>
		{
			public bool Equals(LinkRecord x, LinkRecord y)
			{
				if (ReferenceEquals(x, y))
					return true;

				if (x == null || y == null)
					return false;

				return x.Description.Equals(y.Description);
			}

			public int GetHashCode(LinkRecord obj)
			{
				return obj != null ? obj.Description.GetHashCode() : 0;
			}
		}

		public CommonServices(ISessionFactory sessionFactory)
			: base(sessionFactory) { }

		public LinkRecordContract FindLinkRecord(string url, IrcName channel)
		{
			var chanName = channel.LowercaseName;

			return HandleQuery(session =>
			{
				var record = session
					.Query<LinkRecord>()
					.OrderBy(l => l.Date)
					.FirstOrDefault(l => l.Channel == chanName && l.Url.Contains(url));

				return (record != null ? new LinkRecordContract(record) : null);
			});
		}

		public LinkRecordContract[] GetRecords(IrcName channel, string nick, int start, int maxCount)
		{
			var chanName = channel.LowercaseName;
			nick = nick ?? string.Empty;

			return HandleQuery(session =>
			{
				LinkRecord rec = null;
				var q = session.QueryOver(() => rec)
					.Where(l => l.Channel == chanName && (nick == string.Empty || l.Nick == nick))
					.OrderBy(m => m.Date).Desc
					.Skip(start)
					.Take(maxCount)
					.Cacheable();

				return q.List()
					.Distinct(new LineEqualityComparer())
					.Select(l => new LinkRecordContract(l))
					.ToArray();
			});
		}

		public LinkRecordContract[] RecordLinks(LinkRecordContract[] linkRecordContracts, IrcName channel)
		{
			ParamIs.NotNull(() => linkRecordContracts);

			var recordUrls = linkRecordContracts.Select(l => l.Url).ToArray();
			var chanName = channel.LowercaseName;

			return HandleTransaction(session =>
			{
				var existingRecords = session
					.Query<LinkRecord>()
					.Where(l => l.Channel == chanName && recordUrls.Contains(l.Url))
					.ToArray();

				var newRecords =
					from recordContract in linkRecordContracts
					where !existingRecords.Any(e => e.Url.Equals(recordContract.Url, StringComparison.InvariantCultureIgnoreCase))
					select new LinkRecord(recordContract);

				foreach (var newRecord in newRecords)
					session.Save(newRecord);

				return existingRecords.Select(r => new LinkRecordContract(r)).ToArray();
			});
		}
	}
}
