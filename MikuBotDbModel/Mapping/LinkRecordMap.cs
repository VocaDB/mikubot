using FluentNHibernate.Mapping;
using MikuBot.DbModel.Model;

namespace MikuBot.DbModel.Mapping {

	public class LinkRecordMap : ClassMap<LinkRecord> {

		public LinkRecordMap() {

			Cache.NonStrictReadWrite();

			Id(m => m.Id);
			Map(m => m.Channel).Length(50).Not.Nullable();
			Map(m => m.Date).Not.Nullable();
			Map(m => m.Description).Length(512).Not.Nullable();
			Map(m => m.Nick).Length(50).Not.Nullable();
			Map(m => m.Title).Length(500).Not.Nullable();
			Map(m => m.Url).Length(255).Not.Nullable();

		}

	}

}
