using System.Web.Mvc;
using System.Web.Routing;
using MikuBot.DbModel.Services;
using NHibernate;

namespace MikuBot.Site
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		private static ISessionFactory sessionFactory;
		private const string sessionFactoryLock = "lock";

		public static CommonServices Services
		{
			get
			{
				return new CommonServices(SessionFactory);
			}
		}

		public static ISessionFactory SessionFactory
		{
			get
			{
				lock (sessionFactoryLock)
				{
					if (sessionFactory == null)
						sessionFactory = DatabaseConfiguration.BuildSessionFactory();
				}

				return sessionFactory;
			}
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}
}