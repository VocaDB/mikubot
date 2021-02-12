using System;
using System.Collections;
using System.Configuration.Install;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using log4net;
using MikuBot.Services;

namespace MikuBot
{
	class Program
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private static Bot bot;
		private static MikuBotServiceHost serviceHost;

		public static Bot Bot
		{
			get { return bot; }
		}

		static void Main(string[] args)
		{
			Thread.CurrentThread.Name = "Boot";
			log4net.Config.XmlConfigurator.Configure();

			if (args.Length >= 1 && args[0] == "/service")
			{
				var servName = args.Length >= 2 ? args[1] : "MikuBot";

				log.Info("Running as service '" + servName + "'");

				System.ServiceProcess.ServiceBase.Run(new WinService(servName));
			}
			else if (args.Length >= 1 && args[0] == "/install")
			{
				InstallService();
			}
			else
			{
				log.Info("Running in standalone mode.");
				RunBot();
			}
		}

		public static void RunBot()
		{
			Application.ApplicationExit += Application_ApplicationExit;
			Application.ThreadException += Application_ThreadException;
			if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
				Thread.CurrentThread.Name = "Main";

			var config = new Config();

			if (config.EnableServices)
				serviceHost = new MikuBotServiceHost();

			try
			{
				Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(config.Culture);
			}
			catch (ArgumentException x)
			{
				log.Error("Unable to set culture", x);
			}

			bot = new Bot(config);
			bot.Run();
		}

		private static void Application_ApplicationExit(object sender, System.EventArgs e)
		{
			if (serviceHost != null)
				serviceHost.Dispose();
		}

		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			log.Fatal("Unhandled exception", e.Exception);
		}

		private static AssemblyInstaller GetInstaller()
		{
			var installer = new AssemblyInstaller(
				typeof(Program).Assembly, null);
			installer.UseNewContext = true;
			return installer;
		}

		private static void InstallService()
		{
			using (AssemblyInstaller installer = GetInstaller())
			{
				IDictionary state = new Hashtable();
				try
				{
					installer.Install(state);
					installer.Commit(state);
				}
				catch
				{
					try
					{
						installer.Rollback(state);
					}
					catch { }
					throw;
				}
			}
		}
	}
}
