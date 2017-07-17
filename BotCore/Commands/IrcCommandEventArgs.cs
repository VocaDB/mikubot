using System;

namespace MikuBot.Commands {

	public class IrcCommandEventArgs<T> : EventArgs where T : IrcCommand {

		public IrcCommandEventArgs(T message) {
			this.Message = message;
		}

		public T Message { get; private set; }

	}

	public static class IrcCommandEventArgsFactory {

		public static IrcCommandEventArgs<T> Create<T>(T message) where T : IrcCommand {

			return new IrcCommandEventArgs<T>(message);

		}

	}

}
