using NHibernate;

namespace MikuBot.DbModel.Services
{
	public class DbServicesManager
	{
		private ISessionFactory sessionFactory;
		private const string sessionFactoryLock = "sessionFactory";

		private ISessionFactory SessionFactory
		{
			get
			{
				lock (sessionFactoryLock)
				{
					return sessionFactory ?? (sessionFactory = DatabaseConfiguration.BuildSessionFactory());
				}
			}
		}

		public CommonServices Common
		{
			get
			{
				return new CommonServices(SessionFactory);
			}
		}
	}
}
