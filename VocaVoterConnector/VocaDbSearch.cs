using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector;
using MikuBot.VocaDBConnector.Helpers;
using MikuBot.VocaDBConnector.VocaDbServices;

namespace MikuBot.VocaVoterConnector
{
	public class VocaDbSearch : MsgCommandModuleBase
	{
		private class Result
		{
			public Result(string name, string line)
			{
				Name = name;
				Line = line;
			}

			public string Line { get; private set; }

			public string Name { get; private set; }
		}

		private VocaVoterConnectorFile connectorFile;

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			var query = cmd.BotCommand.CommandString;

			var entries = connectorFile.CallClient(client => client.FindAll(query, 5, ContentLanguagePreference.English));

			if (entries.TotalCount == 0)
			{
				reply.Msg("No results.");
				return;
			}

			reply.Msg(string.Format("Found {0} result(s) (displaying first 5).", entries.TotalCount));
			var config = connectorFile.Config;

			var results = entries.Items.Select(a => new Result(a.Name, EntryFormattingHelper.FormatEntryWithUrl(a, config)))
				.OrderBy(n => n.Name).Take(5);

			foreach (var result in results)
			{
				reply.Msg(result.Line);
			}
		}

		public override string Name
		{
			get { return "Search"; }
		}

		public override int CooldownChannelMs
		{
			get { return 1000; }
		}

		public override int CooldownUserMs
		{
			get { return 10000; }
		}

		public override string CommandDescription
		{
			get { return "Searches all VocaDB entries by name."; }
		}

		public override string UsageHelp
		{
			get { return "vocadbsearch <term>"; }
		}

		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}
	}
}
