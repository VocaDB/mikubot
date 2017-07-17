namespace MikuBot.Modules {

	public abstract class ModuleFileBase : IModuleFile {

		public virtual void OnLoading(IBotContext bot) {}

		public virtual void OnLoaded(IBotContext bot) {}

		public virtual void OnModuleLoaded(IModule module, IBotContext bot) {}

	}

}
