namespace MikuBot {

	public class Receiver : IReceiver {

		private readonly IrcName destination;
		private readonly IrcWriter writer;

		public Receiver(IrcWriter writer, IrcName destination) {
			this.writer = writer;
			this.destination = destination;
		}

		public IrcName Destination => destination;

		public bool IsChannel => destination.IsChannel;

		public void Action(string text) {
			writer.Action(destination.Name, text);
		}

		public void Msg(string text) {
			writer.Msg(destination.Name, text);
		}

		public void Notice(string text) {
			writer.Notice(destination.Name, text);
		}

	}
}
