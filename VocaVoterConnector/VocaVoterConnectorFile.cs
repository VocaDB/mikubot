using System;
using System.Threading.Tasks;
using MikuBot.Modules;
using MikuBot.VocaVoterConnector;

namespace MikuBot.VocaDBConnector
{
	public class VocaVoterConnectorFile : ModuleFileBase
	{
		public VocaDbConfig Config { get; private set; }

		public void CallClient(ClientType clientType, Action<VocaDbClient> clientFunc)
		{
			using var client = CreateClient(clientType);
			clientFunc(client);
		}

		public T CallClient<T>(ClientType clientType, Func<VocaDbClient, T> clientFunc)
		{
			using var client = CreateClient(clientType);
			return clientFunc(client);
		}

		public async Task<T> CallClientAsync<T>(ClientType clientType, Func<VocaDbClient, Task<T>> clientFunc)
		{
			using var client = CreateClient(clientType);
			return await clientFunc(client);
		}

		public void CallClient(Action<VocaDbClient> clientFunc)
		{
			using var client = CreateClient(ClientType.VocaDb);
			clientFunc(client);
		}

		public T CallClient<T>(Func<VocaDbClient, T> clientFunc)
		{
			using var client = CreateClient(ClientType.VocaDb);
			return clientFunc(client);
		}

		public async Task<T> CallClientAsync<T>(Func<VocaDbClient, Task<T>> clientFunc, ClientType clientType = ClientType.VocaDb)
		{
			using var client = CreateClient(clientType);
			return await clientFunc(client);
		}

		private VocaDbClient CreateClient(ClientType clientType)
		{
			var endpoint = clientType switch
			{
				ClientType.VocaDb => Config.VocaDBUrl,
				ClientType.UtaiteDb => Config.UtaiteDBUrl,
				_ => throw new ArgumentException(),
			} + "api";
			return new VocaDbClient(endpoint);
		}

		public override void OnLoading(IBotContext bot)
		{
			Config = new VocaDbConfig(bot.Config);
		}
	}

	public enum ClientType
	{
		VocaDb,
		UtaiteDb
	}
}
