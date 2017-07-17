using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;
using MikuBot.Commands;

namespace MikuBot {

	public static class BotHelper {

		private static readonly string meiko = 
			"Sorry...I don't know you and " + ColorCode.Red + "Meiko-sensei" + Formatting.Normal + " says I shouldn't talk to strangers.";

		private static readonly StringCollection authorizationRequired = new StringCollection {
			meiko,
			meiko,
			meiko,
			"Who are you? O.o",
			"UNAUTHORIZED ACCESS - YOU WILL BE KILLED",
			"I don't think so."
		};

		private static readonly ILog log = LogManager.GetLogger(typeof (BotHelper));

		public static string AttributeValueOrEmpty(XElement element, XName attributeName) {

			var att = element.Attribute(attributeName);

			return (att != null ? att.Value : string.Empty);

		}

		public static bool CheckAuthenticated(IrcCommand cmd, IBotContext bot) {

			return CheckAuthenticated(cmd, bot, BotUserLevel.Admin);

		}

		public static bool CheckAuthenticated(IrcCommand cmd, IBotContext bot, BotUserLevel minLevel) {

			if (cmd.Sender.UserLevel < minLevel) {
				var response = ChooseRandom(authorizationRequired);
				bot.Writer.Msg(cmd.ChannelOrSenderNick, response);
				return false;
			}

			return true;

		}

		public static string ChooseRandom(StringCollection vals) {
			ParamIs.NotNull(() => vals);
			return vals[new Random().Next(vals.Count)];
		}

		public static string FormatTimeSpan(TimeSpan timeSpan) {

			if (timeSpan.TotalSeconds < 1)
				return "less than a second";

			if (timeSpan.TotalMinutes < 1)
				return "less than a minute";

			if (timeSpan.TotalMinutes < 60)
				return Math.Floor(timeSpan.TotalMinutes) + " minute(s)";

			return string.Format("{0} hour(s) and {1} minute(s)", Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);

		}

		public static Uri[] GetUris(string text) {
			
			var regex = new Regex(@"http[s]?\:[a-zA-Z0-9_\#\-\.\:\/\%\?\&\=\+]+");

			var matches = regex.Matches(text);

			var uris = new List<Uri>();

			foreach (var match in matches) {

				Uri uri;

				if (Uri.TryCreate(match.ToString(), UriKind.Absolute, out uri))
					uris.Add(uri);

			}

			return uris.ToArray();

		}

		public static void HandleTaskException(Task task) {

			log.Warn("Task caused an exception", task.Exception);

		}

		public static void JoinAll(string[] channels, IrcWriter writer) {
			
			foreach (var channel in channels)
				writer.Join(channel);

		}

		public static bool IsOwnNick(IrcName nick, IBotContext bot) {
			
			ParamIs.NotNull(() => bot);

			return bot.OwnNick.Equals(nick);

		}

		public static DateTime? ParseDateTimeOrNull(string str) {

			DateTime parsed;
			if (DateTime.TryParse(str, out parsed))
				return parsed;

			return null;

		}

	}

}
