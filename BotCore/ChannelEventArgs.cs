using System;

namespace MikuBot {

	public class ChannelEventArgs : EventArgs {

		public ChannelEventArgs(IrcName channel) {
			Channel = channel;
		}

		public IrcName Channel { get; private set; }

	}

}
