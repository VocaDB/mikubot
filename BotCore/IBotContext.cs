using System;
using MikuBot.Modules;
using MikuBot.Security;

namespace MikuBot {

	public interface IBotContext {

		IAuthenticator Authenticator { get; }

		IChannelManager ChannelManager { get; }

		IConfig Config { get; }

		object DbServices { get; }

		string HighlightShortcut { get; }

		IIgnoredNickList IgnoredNickList { get; }

		bool IsConnected { get; }

		IModuleManager ModuleManager { get; }

		INickManager NickManager { get; }

		IrcName OwnNick { get; }

		DateTime StartupTime { get; }

		IrcWriter Writer { get; }

		void Reconnect();

	}
}
