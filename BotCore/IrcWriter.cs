using System.IO;
using log4net;

namespace MikuBot {

	public class IrcWriter {

		private static readonly ILog log = LogManager.GetLogger(typeof(IrcWriter));
		private readonly TextWriter writer;

		public delegate void SendTextDelegate(string text);

		public event SendTextDelegate SendText;

		protected void OnSendText(string text) {

			if (SendText != null)
				SendText(text);

		}

		public IrcWriter(TextWriter writer) {

			ParamIs.NotNull(() => writer);

			this.writer = writer;

		}

		public void Action(string destination, string text) {

			Msg(destination, string.Format("\u0001ACTION {0}\u0001", text));

		}

		public void Action(IrcName destination, string text) {

			Action(destination.Name, text);

		}

		public void AuthorizationRequired(string user) {
			
			Msg(user, "Sorry...I don't know you and Meiko-sensei says I shouldn't talk to strangers");

		}

		public void Join(string channel) {

			if (string.IsNullOrEmpty(channel)) {
				log.Warn("Attempting to join a channel without a name");
				return;				
			}

			Send("JOIN " + channel);

		}

		public void Msg(string destination, string text) {

			if (string.IsNullOrEmpty(destination)) {
				log.Warn("Attempting to send text without destination");
				return;
			}

			Send(string.Format("PRIVMSG {0} :{1}", destination, text));

		}

		public void Msg(IrcName destination, string text) {

			Msg(destination.Name, text);

		}

		public void Nick(string nick) {

			if (string.IsNullOrEmpty(nick)) {
				log.Warn("Attempting to send empty nick");
				return;
			}
	
			Send("NICK " + nick);

		}

		public void Notice(string destination, string text) {

			if (string.IsNullOrEmpty(destination)) {
				log.Warn("Attempting to send text without destination");
				return;
			}

			Send(string.Format("NOTICE {0} :{1}", destination, text));

		}

		public void Notice(IrcName destination, string text) {

			Notice(destination.Name, text);

		}

		public void Part(string channel) {
			Send("PART " + channel);
		}

		public void Send(string text) {

			log.Info("Sending " + text);

			try {
				writer.WriteLine(text);
				writer.Flush();
			} catch (IOException x) {
				log.Error("Unable to send message", x);
				return;
			}

			OnSendText(text);

		}

		public void User(string username, string realname) {

			if (string.IsNullOrEmpty(username)) {
				log.Warn("Attempting to send USER message without username");
				return;
			}

			if (string.IsNullOrEmpty(realname)) {
				log.Warn("Attempting to send USER message without realname");
				return;
			}

			Send(string.Format("USER {0} 0 * :{1}", username, realname));

		}

	}
}
