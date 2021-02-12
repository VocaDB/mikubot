using MikuBot.ExtraPlugins.Helpers;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class ExtraPluginsModuleFile : ModuleFileBase
	{
		public ExtraPluginsModuleFile()
		{
			MessageBuffer = new MessageBuffer();
			UserActivityMonitor = new UserActivityMonitor();
		}

		public MessageBuffer MessageBuffer { get; private set; }

		public UserActivityMonitor UserActivityMonitor { get; private set; }
	}
}
