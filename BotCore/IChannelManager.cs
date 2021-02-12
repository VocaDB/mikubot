using System.Collections.Generic;

namespace MikuBot
{
	public interface IChannelManager
	{
		IEnumerable<IrcName> Channels { get; }

		void Autojoin();

		bool IsOnChannel(IrcName channel);

		void Join(IrcName channel);

		void JoinAll(IEnumerable<IrcName> channels);

		void Part(IrcName channel);
	}
}
