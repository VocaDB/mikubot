using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MikuBot.Services
{
	public class MikuBotServiceHost : IDisposable
	{
		private ServiceHost adminServiceHost;

		private void Init()
		{
			adminServiceHost = new ServiceHost(typeof(AdminService));
			adminServiceHost.Open();
		}

		public MikuBotServiceHost()
		{
			new TaskFactory().StartNew(Init);
		}

		public void Dispose()
		{
			adminServiceHost.Close();
		}
	}
}
