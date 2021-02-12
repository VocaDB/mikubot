using System;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;

namespace MikuBot
{
	public class WinService : ServiceBase
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(WinService));

		public WinService(string servName)
		{
			this.ServiceName = servName;
			this.CanStop = true;
			this.CanPauseAndContinue = false;
			this.AutoLog = false;
		}

		protected override void OnStart(string[] args)
		{
			log.Info("Starting service '" + ServiceName + "'");
			Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;

			new Task(Program.RunBot).Start();
		}

		protected override void OnStop()
		{
			log.Info("Stopping service");

			Program.Bot.Quit();
		}
	}
}
