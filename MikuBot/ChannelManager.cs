using System;
using System.Collections.Generic;
using System.Linq;
using MikuBot.Commands;
using MikuBot.Commands.Numeric;

namespace MikuBot {

	public class ChannelManager : IChannelManager {

		private readonly HashSet<IrcName> autojoinChannels = new HashSet<IrcName>();
		private readonly Dictionary<IrcName, ChannelStatus> channels = new Dictionary<IrcName, ChannelStatus>();
		private readonly IBotContext botContext;

		private IrcWriter Writer {
			get { return botContext.Writer; }
		}

		private void AddUser(IrcName chanName, Hostmask user) {

			RunChanCmd(chanName, chan => chan.AddUser(user));

		}

		private void RemoveUser(IrcName chanName, Hostmask user) {

			RunChanCmd(chanName, chan => chan.RemoveUser(user));

		}

		private void RemoveUserByNick(IrcName chanName, IrcName nick) {

			RunChanCmd(chanName, chan => chan.RemoveUserByNick(nick));

		}

		private void RunChanCmd(IrcName chanName, Action<ChannelStatus> func) {

			if (!channels.ContainsKey(chanName))
				return;

			var chan = channels[chanName];
			func(chan);

		}

		private void OnConnected(object sender, EventArgs e) {

			Autojoin();

		}

		private void OnDisconnected(object sender, EventArgs e) {

			OnLeftAllChannels();

		}

		private void OnInviteMessage(object sender, IrcCommandEventArgs<InviteCommand> e) {

			var cmd = e.Message;

			if (cmd.ReceiverNick != botContext.OwnNick || !BotHelper.CheckAuthenticated(cmd, botContext, BotUserLevel.Manager))
				return;

			Join(cmd.Channel);

		}

		protected void OnJoined(object sender, ChannelEventArgs e) {

			if (Joined != null)
				Joined(sender, e);

		}

		private void OnJoinMessage(object sender, IrcCommandEventArgs<JoinCommand> e) {

			AddUser(e.Message.Channel, e.Message.JoinedUser.Host);

		}

		private void OnKickMessage(object sender, IrcCommandEventArgs<KickCommand> e) {

			var cmd = e.Message;

			if (cmd.KickedUserIsSelf)
				OnLeftChannel(cmd.Channel);
			else
				RemoveUserByNick(cmd.Channel, cmd.KickedUserNick);

		}

		private void OnKillMessage(object sender, IrcCommandEventArgs<KillMessage> e) {

			foreach (var chan in channels)
				chan.Value.RemoveUserByNick(e.Message.KilledNick);

		}

		private void OnNickMessage(object sender, IrcCommandEventArgs<NickMessage> e) {

			foreach (var chan in channels)
				chan.Value.ChangeUserNick(e.Message.SenderHost, e.Message.NewNick);

		}

		private void NumericReplyMessage(object sender, IrcCommandEventArgs<NumericReply> e) {

			if (e.Message is WhoReply) {

				var whoReply = (WhoReply)e.Message;

				AddUser(whoReply.Channel, whoReply.Hostmask);

			}

		}

		private void OnPartMessage(object sender, IrcCommandEventArgs<PartMessage> e) {

			RemoveUser(e.Message.Channel, e.Message.PartedUser.Host);

		}

		private void OnQuitMessage(object sender, IrcCommandEventArgs<QuitMessage> e) {
			
			foreach (var chan in channels)
				chan.Value.RemoveUser(e.Message.QuitUser.Host);

		}

		public event EventHandler<ChannelEventArgs> Joined;

		public ChannelManager(Bot bot, IEnumerable<IrcName> autojoinChannels) {

			ParamIs.NotNull(() => bot);

			this.botContext = bot;

			foreach (var channel in autojoinChannels)
				this.autojoinChannels.Add(channel);

			bot.Connected += OnConnected;
			bot.JoinMessage += OnJoinMessage;
			bot.Disconnected += OnDisconnected;
			bot.InviteMessage += OnInviteMessage;
			bot.KickMessage += OnKickMessage;
			bot.KillMessage += OnKillMessage;
			bot.NickMessage += OnNickMessage;
			bot.NumericReplyMessage += NumericReplyMessage;
			bot.PartMessage += OnPartMessage;
			bot.QuitMessage += OnQuitMessage;

		}

		public IEnumerable<IrcName> Channels {
			get {
				return channels.Select(c => c.Key);
			}
		}

		public void Autojoin() {

			var join = autojoinChannels.Where(c => !IsOnChannel(c));

			JoinAll(join);

		}

		public bool IsOnChannel(IrcName channel) {

			return Channels.Contains(channel);

		}

		public void Join(IrcName channel) {

			if (!channel.IsChannel)
				return;

			autojoinChannels.Add(channel);

			Writer.Join(channel.Name);
			Writer.Send("WHO " + channel);

			if (!channels.ContainsKey(channel)) {
				channels.Add(channel, new ChannelStatus(channel));
				OnJoined(this, new ChannelEventArgs(channel));
			}

		}

		public void JoinAll(IEnumerable<IrcName> channels) {
			
			ParamIs.NotNull(() => channels);

			foreach (var channel in channels)
				Join(channel);

		}

		public void Part(IrcName channel) {

			if (!channel.IsChannel)
				return;

			autojoinChannels.Remove(channel);
			channels.Remove(channel);

			Writer.Part(channel.Name);

		}

		private void OnLeftAllChannels() {
			
			channels.Clear();

		}

		private void OnLeftChannel(IrcName channel) {

			channels.Remove(channel);

		}

	}

}
