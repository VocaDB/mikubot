using MikuBot.VocaDBConnector;

namespace MikuBot.VocaVoterConnector
{
	public class VocaDbConfig
	{
		private readonly IConfig config;

		public VocaDbConfig(IConfig config)
		{
			ParamIs.NotNull(() => config);
			this.config = config;
		}

		public string GetSiteUrl(ClientType site)
		{
			return (site == ClientType.VocaDb ? VocaDBUrl : UtaiteDBUrl);
		}

		public string UtaiteDBUrl
		{
			get
			{
				return config.GetString("UtaiteDbUrl");
			}
		}

		public string VocaDBUrl
		{
			get
			{
				return config.GetString("VocaDbUrl");
			}
		}
	}
}
