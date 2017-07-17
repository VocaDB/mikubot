using System;
using MikuBot.Commands;
using MikuBot.Commands.Numeric;

namespace MikuBot {

	public class UserManager {

		private readonly UserCollection users = new UserCollection();

		public void OnDisconnected(object sender, EventArgs e) {
			users.Clear();
		}

		private void OnJoinMessage(object sender, IrcCommandEventArgs<JoinCommand> e) {

			users.Add(e.Message.JoinedUser.Host);

		}

		private void OnKickMessage(object sender, IrcCommandEventArgs<KickCommand> e) {

			users.RemoveByNick(e.Message.KickedUserNick);

		}

		private void OnKillMessage(object sender, IrcCommandEventArgs<KillMessage> e) {

			users.RemoveByNick(e.Message.KilledNick);

		}

		private void OnNickMessage(object sender, IrcCommandEventArgs<NickMessage> e) {

			users.ChangeNick(e.Message.OriginalNick, e.Message.NewNick);

		}

		private void NumericReplyMessage(object sender, IrcCommandEventArgs<NumericReply> e) {

			if (e.Message is WhoReply) {

				var whoReply = (WhoReply)e.Message;

				users.Add(whoReply.Hostmask);

			}

		}

		private void OnPartMessage(object sender, IrcCommandEventArgs<PartMessage> e) {

			users.Remove(e.Message.PartedUser.Host);

		}

		private void OnQuitMessage(object sender, IrcCommandEventArgs<QuitMessage> e) {

			users.Remove(e.Message.QuitUser.Host);

		}

		public UserManager(Bot bot) {
			
			ParamIs.NotNull(() => bot);

			bot.JoinMessage += OnJoinMessage;
			bot.Disconnected += OnDisconnected;
			bot.KickMessage += OnKickMessage;
			bot.KillMessage += OnKillMessage;
			bot.NickMessage += OnNickMessage;
			bot.NumericReplyMessage += NumericReplyMessage;
			bot.PartMessage += OnPartMessage;
			bot.QuitMessage += OnQuitMessage;


		}

		public IReadOnlyUserCollection Users {
			get { return users; }
		}

	}
}
