using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MikuBot.DbModel.Services;
using log4net;
using MikuBot.Commands;
using MikuBot.Modules;
using System.Windows.Forms;
using MikuBot.Security;

namespace MikuBot
{
	public class Bot : IBotContext
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Bot));

		private readonly Authenticator authenticator;
		private readonly ChannelManager channelManager;
		private readonly Config config;
		private bool connected;
		private DbServicesManager dbServices = new DbServicesManager();
		private bool disconnect;
		private readonly IgnoredNickList ignoredNickList = new IgnoredNickList();
		private DateTime lastMsgTime;
		private readonly ModuleManager moduleManager;
		private readonly NickManager nickManager;
		private bool run;
		private DateTime startupTime;
		private readonly UserManager userManager;
		private IrcWriter writer;

		private void HandleCommand(IrcCommand cmd)
		{
			if (cmd is PingCommand)
			{
				HandlePing((PingCommand)cmd);
			}

			var sender = cmd.Sender;
			if (sender != null && sender.UserLevel < BotUserLevel.Manager && ignoredNickList.IsIgnored(sender.Host))
			{
				return;
			}

			if (cmd is NumericReply)
				HandleNumericReply((NumericReply)cmd);

			if (cmd is InviteCommand)
				OnInviteMessage((InviteCommand)cmd);

			if (cmd is JoinCommand)
				OnJoinMessage((JoinCommand)cmd);

			if (cmd is KickCommand)
				OnKickMessage((KickCommand)cmd);

			if (cmd is KillMessage)
				OnKillMessage((KillMessage)cmd);

			if (cmd is NickMessage)
				OnNickMessage((NickMessage)cmd);

			if (cmd is PartMessage)
				OnPartMessage((PartMessage)cmd);

			if (cmd is QuitMessage)
				OnQuitMessage((QuitMessage)cmd);

			moduleManager.HandleCommand(cmd, this);

			//if (songSearch != null)
			//	songSearch.HandleCommand(cmd, this);

			//var nicoParser = new NicoParser();
			//nicoParser.HandleCommand(cmd, this);
		}

		private void HandleLine(string inputLine)
		{
			Console.WriteLine("<-" + inputLine);
			//logWriter.WriteLine(inputLine, this);

			moduleManager.HandleLine(inputLine, this);

			var cmd = IrcCommand.Parse(inputLine, this);

			HandleCommand(cmd);
		}

		private void HandleNumericReply(NumericReply cmd)
		{
			if (cmd.CommonReplyCode == ReplyCode.ERR_NICKNAMEINUSE && cmd.ParamCollection.ParamOrEmpty(0) == "*")
			{
				log.Warn("Nickname in use; trying another one");
				nickManager.Next();
				Thread.Sleep(3000);
			}

			if (cmd.CommonReplyCode == ReplyCode.RPL_ENDOFMOTD || cmd.CommonReplyCode == ReplyCode.ERR_NOMOTD)
			{
				OnConnected();
			}

			OnNumericReplyMessage(cmd);
		}

		private void HandlePing(PingCommand cmd)
		{
			writer.Send("PONG " + cmd.Origin);
		}

		private void Login()
		{
			var nick = nickManager.Current.Name;

			Writer.Nick(nick);
			Writer.User(nick, nick);
		}

		private void OnConnected()
		{
			if (Connected != null)
				Connected(this, EventArgs.Empty);
		}

		private void OnDisconnected()
		{
			if (Disconnected != null)
				Disconnected(this, EventArgs.Empty);
		}

		private void OnInviteMessage(InviteCommand msg)
		{
			if (InviteMessage != null)
				InviteMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnJoinMessage(JoinCommand msg)
		{
			if (JoinMessage != null)
				JoinMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnKickMessage(KickCommand msg)
		{
			if (KickMessage != null)
				KickMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnKillMessage(KillMessage msg)
		{
			if (KillMessage != null)
				KillMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnNickMessage(NickMessage msg)
		{
			if (NickMessage != null)
				NickMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnNumericReplyMessage(NumericReply msg)
		{
			if (NumericReplyMessage != null)
				NumericReplyMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnPartMessage(PartMessage msg)
		{
			if (PartMessage != null)
				PartMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		private void OnQuitMessage(QuitMessage msg)
		{
			if (QuitMessage != null)
				QuitMessage(this, IrcCommandEventArgsFactory.Create(msg));
		}

		/// <summary>
		/// The bot has finished connecting to a server.
		/// </summary>
		public event EventHandler Connected;

		/// <summary>
		/// The bot has disconnected from the server.
		/// </summary>
		public event EventHandler Disconnected;

		public event EventHandler<IrcCommandEventArgs<InviteCommand>> InviteMessage;
		public event EventHandler<IrcCommandEventArgs<JoinCommand>> JoinMessage;
		public event EventHandler<IrcCommandEventArgs<KickCommand>> KickMessage;
		public event EventHandler<IrcCommandEventArgs<KillMessage>> KillMessage;
		public event EventHandler<IrcCommandEventArgs<NickMessage>> NickMessage;
		public event EventHandler<IrcCommandEventArgs<NumericReply>> NumericReplyMessage;
		public event EventHandler<IrcCommandEventArgs<PartMessage>> PartMessage;
		public event EventHandler<IrcCommandEventArgs<QuitMessage>> QuitMessage;

		public Bot(Config config)
		{
			ParamIs.NotNull(() => config);

			startupTime = DateTime.Now;
			this.config = config;

			authenticator = new Authenticator(config);
			nickManager = new NickManager(this, config.Nicks);
			userManager = new UserManager(this);
			channelManager = new ChannelManager(this, config.AutoJoinChannels);
			moduleManager = new ModuleManager(this);
			moduleManager.DisablePluginModules(config.DisableModules, true);
		}

		public IAuthenticator Authenticator
		{
			get { return authenticator; }
		}

		public IChannelManager ChannelManager
		{
			get { return channelManager; }
		}

		public Config Config
		{
			get { return config; }
		}

		IConfig IBotContext.Config
		{
			get { return Config; }
		}

		object IBotContext.DbServices
		{
			get { return DbServices; }
		}

		public DbServicesManager DbServices
		{
			get { return dbServices; }
		}

		public string HighlightShortcut
		{
			get
			{
				return config.HighlightShortcut;
			}
		}

		public IIgnoredNickList IgnoredNickList
		{
			get { return ignoredNickList; }
		}

		public bool IsConnected
		{
			get { return connected; }
		}

		public ModuleManager ModuleManager
		{
			get { return moduleManager; }
		}

		IModuleManager IBotContext.ModuleManager
		{
			get { return ModuleManager; }
		}

		public INickManager NickManager
		{
			get { return nickManager; }
		}

		public IrcName OwnNick
		{
			get
			{
				return nickManager.Current;
			}
		}

		public DateTime StartupTime
		{
			get { return startupTime; }
		}

		public UserManager UserManager
		{
			get { return userManager; }
		}

		public IrcWriter Writer
		{
			get { return writer; }
		}

		public void Reconnect()
		{
			log.Info("Reconnecting");
			disconnect = true;
		}

		public void Quit()
		{
			log.Info("Quitting");
			run = false;
		}

		private bool ReadLine(StreamReader reader)
		{
			string inputLine = null;

			try
			{
				inputLine = reader.ReadLine();
			}
			catch (IOException x)
			{
				log.Warn("Unable to read from stream; reconnecting", x);
				Thread.Sleep(3000);
				return false;
			}

			if (inputLine != null)
			{
				HandleLine(inputLine);

				lastMsgTime = DateTime.Now;
			}
			else
			{
				if (lastMsgTime + config.PingTimeout < DateTime.Now)
				{
					log.Warn("Ping timeout reached; reconnecting");
					disconnect = true;
				}
			}

			return true;
		}

		private void writer_SendText(string text)
		{
			moduleManager.OnMessageSent(text, this);
		}

		public void Run()
		{
			//var linkFile = new LogFileLinkReader(DbPlugins.DbPluginsModule.Services, @"C:\Documents\mikuchan.log");
			//linkFile.ReadLinks();

			Console.WriteLine("MikuBot {0} starting up...", Application.ProductVersion);

			run = true;

			if (Config.AutoLoadModules)
			{
				moduleManager.LoadModules(this, new LogReceiver(), Config.Modules);
			}

			while (run)
			{
				disconnect = false;
				TcpClient client = null;

				try
				{
					try
					{
						client = new TcpClient(config.Server, config.Port);
					}
					catch (SocketException x)
					{
						log.Error("Unable to create TCP client; retrying", x);
						Thread.Sleep(3000);
						continue;
					}

					using (var stream = client.GetStream())
					using (var reader = new StreamReader(stream))
					using (var streamWriter = TextWriter.Synchronized(new StreamWriter(stream)))
					{
						connected = true;
						stream.ReadTimeout = (int)config.PingTimeout.TotalMilliseconds;
						writer = new IrcWriter(streamWriter);
						writer.SendText += writer_SendText;

						Login();

						while (run && !disconnect && client.Connected && ReadLine(reader)) { }
					}
				}
				finally
				{
					connected = false;

					if (client != null)
						client.Close();

					OnDisconnected();
				}
			}
		}
	}
}
