using System;
using MikuBot.DbModel.DataContracts;

namespace MikuBot.DbModel.Model {

	public class LinkRecord {

		private string channel;
		private string description;
		private string nick;
		private string url;
		private string title;

		public LinkRecord() {
			Title = string.Empty;
		}

		public LinkRecord(LinkRecordContract contract) {

			ParamIs.NotNull(() => contract);

			Channel = contract.Channel.LowercaseName;
			Date = contract.Date;
			Description = contract.Description;
			Nick = contract.Nick.Name;
			Title = contract.Title;
			Url = contract.Url;

		}

		/// <summary>
		/// Channel name. Cannot be null or empty. Always lowercased.
		/// </summary>
		public virtual string Channel {
			get { return channel; }
			set {
				ParamIs.NotNullOrEmpty(() => value);
				channel = value;
			}
		}

		public virtual DateTime Date { get; protected set; }

		public virtual string Description {
			get { return description; }
			protected set {
				ParamIs.NotNull(() => value);
				description = value;
			}
		}

		public virtual int Id { get; protected set; }

		/// <summary>
		/// Nickname of the user who posted the link. Cannot be null or empty. This name is in the original format (not lowercased).
		/// </summary>
		public virtual string Nick {
			get { return nick; }
			protected set {
				ParamIs.NotNullOrEmpty(() => value);
				nick = value;
			}
		}

		public virtual string Title {
			get { return title; }
			set {
				ParamIs.NotNull(() => value);
				title = value;
			}
		}

		public virtual string Url {
			get { return url; }
			protected set {
				ParamIs.NotNullOrEmpty(() => value);
				url = value;
			}
		}

	}

}
