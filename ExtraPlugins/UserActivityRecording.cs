using System;
using System.IO;
using MikuBot.Commands;
using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class UserActivityRecording : GenericModuleBase
	{
		private const string activityFile = "UserActivity.xml";
		private readonly TimeSpan saveFrequency = TimeSpan.FromMinutes(5);

		private DateTime lastSaved = DateTime.MinValue;
		private UserActivityMonitor userActivityMonitor;

		private void Save()
		{
			using (var f = File.Create(activityFile))
				userActivityMonitor.Save(f);

			lastSaved = DateTime.Now;
		}

		public override string HelpText
		{
			get { return "Records information on when a specific nickname was last active."; }
		}

		public override void HandleCommand(IrcCommand command, IBotContext bot)
		{
			if (command.Sender == null || !command.ChannelOrSenderNick.IsChannel)
				return;

			userActivityMonitor.Update(command.ChannelOrSenderNick, command.Sender.Nick, command.Command);

			if (lastSaved + saveFrequency < DateTime.Now)
				Save();
		}

		public override void OnDisconnected(IBotContext bot)
		{
			Save();
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			ParamIs.NotNull(() => moduleFile);

			var extraPluginsModuleFile = (ExtraPluginsModuleFile)moduleFile;
			userActivityMonitor = extraPluginsModuleFile.UserActivityMonitor;

			if (File.Exists(activityFile))
			{
				using (var f = File.OpenRead(activityFile))
					userActivityMonitor.Restore(f);

				lastSaved = DateTime.Now;
			}
		}

		public override string Name
		{
			get { return "UserActivityRecording"; }
		}
	}
}
