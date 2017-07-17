using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using log4net;
using MikuBot.Commands;

namespace MikuBot.ExtraPlugins.Helpers {

	public class UserActivityMonitor {

		public class UserActivityEntry {

			public UserActivityEntry(string messageName) {
				Update(messageName);
			}

			public UserActivityEntry(string messageType, DateTime time, DateTime? privMsgTime) {
				MessageType = messageType;
				Time = time;
				PrivMsgTime = privMsgTime;
			}

			public string MessageType { get; private set; }

			public DateTime? PrivMsgTime { get; private set; }

			public DateTime Time { get; private set; }

			public void Update(string messageName) {
				
				ParamIs.NotNull(() => messageName);

				if (messageName.Equals(MsgCommand.MessageName, StringComparison.InvariantCultureIgnoreCase))
					PrivMsgTime = DateTime.Now;

				MessageType = messageName;
				Time = DateTime.Now;

			}

		}

		class ChannelEntry {

			private readonly Dictionary<IrcName, UserActivityEntry> nickTimes = new Dictionary<IrcName, UserActivityEntry>();

			public ChannelEntry(IrcName name, IrcName nick, string messageType) {
				Name = name;
				Update(nick, messageType);
			}

			public ChannelEntry(IrcName name, Dictionary<IrcName, UserActivityEntry> nickTimes) {
				Name = name;
				this.nickTimes = nickTimes;
			}

			public IrcName Name { get; private set; }

			public IEnumerable<KeyValuePair<IrcName, UserActivityEntry>> NickTimes {
				get { return nickTimes; }
			}

			public UserActivityEntry Find(IrcName nick) {

				UserActivityEntry date;

				if (!nickTimes.TryGetValue(nick, out date))
					return null;
				else
					return date;

			}

			public void Update(IrcName nick, string messageType) {

				if (nickTimes.ContainsKey(nick)) {
					nickTimes[nick].Update(messageType);
				} else {
					var entry = new UserActivityEntry(messageType);
					nickTimes.Add(nick, entry);
				}

			}

		}

		private static readonly ILog log = LogManager.GetLogger(typeof(UserActivityMonitor));
		private Dictionary<IrcName, ChannelEntry> channelEntries = new Dictionary<IrcName, ChannelEntry>();

		public UserActivityEntry Find(IrcName channel, IrcName nick) {

			ChannelEntry entry;

			if (channelEntries.TryGetValue(channel, out entry))
				return entry.Find(nick);
			else
				return null;

		}

		public void Restore(Stream input) {
			
			channelEntries.Clear();

			XDocument doc;
			try {
				doc = XDocument.Load(input);
			} catch (XmlException x) {
				log.Warn("Unable to load user activity data", x);
				channelEntries = new Dictionary<IrcName, ChannelEntry>();
				return;
			}

			var root = doc.Element("userActivity");

			channelEntries = root.Elements("channelEntry").Select(e =>				
				new ChannelEntry(new IrcName(e.Attribute("name").Value), 
					e.Elements("nickTime").ToDictionary(n =>
						new IrcName(n.Attribute("name").Value),
						n => new UserActivityEntry(
							n.Attribute("messageType").Value,
							DateTime.Parse(n.Attribute("time").Value),
							BotHelper.ParseDateTimeOrNull(BotHelper.AttributeValueOrEmpty(n, "privMsgTime"))))))
				.Where(c => c.Name.IsChannel).ToDictionary(c => c.Name, c => c);

		}

		public void Save(Stream output) {

			var tree = new XDocument(
				new XElement("userActivity", channelEntries.Select(c =>
					new XElement("channelEntry",
						new XAttribute("name", c.Key), c.Value.NickTimes.Select(n =>
						new XElement("nickTime",
							new XAttribute("name", n.Key),
							new XAttribute("messageType", n.Value.MessageType),
							new XAttribute("time", n.Value.Time),
							new XAttribute("privMsgTime", n.Value.PrivMsgTime != null ? n.Value.PrivMsgTime.Value.ToString("o") : string.Empty)
						))
					)
				))
			);

			tree.Save(output);

		}

		public void Update(IrcName channel, IrcName nick, string messageType) {

			ChannelEntry entry;

			if (channelEntries.TryGetValue(channel, out entry))
				entry.Update(nick, messageType);
			else
				channelEntries.Add(channel, new ChannelEntry(channel, nick, messageType));

		}

	}

}
