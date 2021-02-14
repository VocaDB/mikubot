using System;
using System.ServiceModel;
using System.Threading.Tasks;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.VocaDbServices;
using MikuBot.VocaVoterConnector;

namespace MikuBot.VocaDBConnector
{
	public class VocaVoterConnectorFile : ModuleFileBase
	{
		public VocaDbConfig Config { get; private set; }
		private string endPointAddressUtaiteDb;
		private string endPointAddressVocaDb;

		public void CallClient(ClientType clientType, Action<QueryServiceClient> clientFunc)
		{
			using var client = CreateClient(clientType);
			clientFunc(client);
		}

		public T CallClient<T>(ClientType clientType, Func<QueryServiceClient, T> clientFunc)
		{
			using var client = CreateClient(clientType);
			return clientFunc(client);
		}

		public async Task<T> CallClientAsync<T>(ClientType clientType, Func<QueryServiceClient, Task<T>> clientFunc)
		{
			using var client = CreateClient(clientType);
			return await clientFunc(client);
		}

		public void CallClient(Action<QueryServiceClient> clientFunc)
		{
			using var client = CreateClient(ClientType.VocaDb);
			clientFunc(client);
		}

		public T CallClient<T>(Func<QueryServiceClient, T> clientFunc)
		{
			using var client = CreateClient(ClientType.VocaDb);
			return clientFunc(client);
		}

		public async Task<T> CallClientAsync<T>(Func<QueryServiceClient, Task<T>> clientFunc, ClientType clientType = ClientType.VocaDb)
		{
			using var client = CreateClient(clientType);
			return await clientFunc(client);
		}

		private QueryServiceClient CreateClient(ClientType clientType)
		{
			var endpoint = clientType == ClientType.VocaDb ? endPointAddressVocaDb : endPointAddressUtaiteDb;
			var isSsl = endpoint.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase);

			var binding = new BasicHttpBinding(isSsl ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None) { MaxReceivedMessageSize = 131072 };
			var endPoint = new EndpointAddress(endpoint);

			return new QueryServiceClient(binding, endPoint);
		}

		public override void OnLoading(IBotContext bot)
		{
			Config = new VocaDbConfig(bot.Config);
			endPointAddressUtaiteDb = bot.Config.GetString("UtaiteDbQueryServiceEndPoint");
			endPointAddressVocaDb = bot.Config.GetString("VocaDbQueryServiceEndPoint");
		}
	}

	public enum ClientType
	{
		VocaDb,
		UtaiteDb
	}
}
