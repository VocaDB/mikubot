using MikuBot.DbModel.Services;
using MikuBot.Modules;

namespace MikuBot.DbPlugins {

	public class DbPluginsModuleFile : ModuleFileBase {

		private DbServicesManager dbServices;

		public CommonServices CommonServices {
			get { return dbServices.Common; }
		}

		public override void OnLoaded(IBotContext bot) {
			dbServices = (DbServicesManager)bot.DbServices;
		}

	}

}
