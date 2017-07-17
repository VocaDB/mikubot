using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Text;
using log4net;

namespace MikuBot {

	[RunInstaller(true)]
	public class WinServiceInstaller : Installer {

		private static readonly ILog log = LogManager.GetLogger(typeof(WinServiceInstaller));
		private readonly ServiceProcessInstaller processInstaller;
		private readonly ServiceInstaller serviceInstaller;

		private string ServName {
			get {

				if (Context != null && Context.Parameters != null && Context.Parameters.ContainsKey("servname"))
					return Context.Parameters["servname"];

				if (Environment.GetCommandLineArgs().Length > 2)
					return Environment.GetCommandLineArgs()[2];
				
				return "MikuBot";

			}
		}

		private void BeforeInstallBot(object sender, InstallEventArgs e) {

			log.Info("Installing service named '" + ServName + "'");

			serviceInstaller.ServiceName = ServName;

		}

		private void BeforeUninstallBot(object sender, InstallEventArgs e) {

			log.Info("Uninstalling service");

			serviceInstaller.ServiceName = ServName;

		} 

		public WinServiceInstaller() {

			this.BeforeInstall += BeforeInstallBot;
			this.BeforeUninstall += BeforeUninstallBot;

			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();

			serviceInstaller.StartType = ServiceStartMode.Manual;

			processInstaller.Account = ServiceAccount.LocalSystem;

			serviceInstaller.Description = "MikuBot IRC bot";

			Installers.Add(serviceInstaller);
			Installers.Add(processInstaller);

		}

		public override void Install(System.Collections.IDictionary stateSaver) {

			var path = new StringBuilder(Context.Parameters["assemblypath"]);
			if (path[0] != '"') {
				path.Insert(0, '"');
				path.Append('"');
			}
			path.Append(" /service " + ServName);
			Context.Parameters["assemblypath"] = path.ToString();
			

			base.Install(stateSaver);

		}

	}  
}
