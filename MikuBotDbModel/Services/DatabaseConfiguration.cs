using System;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net;
using NHibernate;

namespace MikuBot.DbModel.Services
{
	public static class DatabaseConfiguration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseConfiguration));

		private static string ConnectionStringName
		{
			get
			{
				return ConfigurationManager.AppSettings["ConnectionStringName"] ?? "Jupiter";
			}
		}

		public static ISessionFactory BuildSessionFactory()
		{
			var config = Fluently.Configure()
				.Database(
					MsSqlConfiguration.MsSql2008
						.ConnectionString(c => c.FromConnectionStringWithKey(ConnectionStringName))
						.MaxFetchDepth(1))
				.ProxyFactoryFactory<NHibernate.ByteCode.Castle.ProxyFactoryFactory>()
				.Cache(c => c
					.ProviderClass<NHibernate.Caches.SysCache2.SysCacheProvider>()
					.UseSecondLevelCache()
					.UseQueryCache()
				)
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()).Conventions.AddAssembly(Assembly.GetExecutingAssembly()));

			try
			{
				return config.BuildSessionFactory();
			}
			catch (ArgumentException x)
			{
				log.Error("Error while building session factory", x);
				throw;
			}
			catch (FluentConfigurationException x)
			{
				log.Error("Error while building session factory", x);
				throw;
			}
		}
	}
}
