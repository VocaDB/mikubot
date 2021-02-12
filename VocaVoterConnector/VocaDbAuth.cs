using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaDBConnector
{
	public class VocaDbAuth : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		private BotUserLevel GetBotUserLevel(UserContract user)
		{
			if (user.GroupId == UserGroupId.Moderator || user.GroupId == UserGroupId.Admin)
				return BotUserLevel.Manager;
			else
				return BotUserLevel.Identified;
		}

		public override string CommandDescription
		{
			get { return "Authenticates users based on VocaDb access key. To authenticate, you need to supply your VocaDb username and Access key (not password!). Send this in PM!"; }
		}

		public override string Name
		{
			get { return "VocaDbAuth"; }
		}

		public override int BotCommandParamCount
		{
			get
			{
				return 2;
			}
		}

		public override string UsageHelp
		{
			get
			{
				return "vocadbauth <VocaDB username> <VocaDB access key>";
			}
		}

		public override void HandleCommand(MsgCommand command, IBotContext bot)
		{
			if (!command.BotCommand.Is(Name, BotCommandMethod.Private))
				return;

			if (!CheckCall(command, bot))
				return;

			var reply = new Receiver(bot.Writer, command.Sender.Nick);
			var name = command.BotCommand.Params[0];
			var key = command.BotCommand.Params[1];

			var result = connectorFile.CallClient(client => client.GetUser(name, key));

			if (result == null)
			{
				reply.Msg("Key not recognized");
				return;
			}

			reply.Msg("You have been authenticated");
			bot.Authenticator.Authenticate(command.SenderHost, key, GetBotUserLevel(result));
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}
	}
}
