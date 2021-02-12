using System;
using System.Linq;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.VocaDBConnector
{
	public class Sing : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		public override int CooldownChannelMs
		{
			get { return 10000; }
		}

		public override int CooldownUserMs
		{
			get { return 30000; }
		}

		public override string CommandDescription
		{
			get { return "Sings a line from a song. If not specified, the song is chosen by random. The lyrics are taken from VocaDB"; }
		}

		public override string Name
		{
			get { return "Sing"; }
		}

		public override string UsageHelp
		{
			get { return "sing [<song name>]"; }
		}

		public override void HandleCommand(MsgCommand command, IBotContext bot)
		{
			if (!CheckCall(command, bot))
				return;

			var receiver = command.Reply(bot.Writer);

			var query = (command.BotCommand.Params.Count > 0 ? command.BotCommand.CommandString : string.Empty);

			var lyrics = connectorFile.CallClient(client => client.GetRandomSongLyrics(query));

			if (lyrics == null)
			{
				receiver.Msg("No results.");
				return;
			}

			var lines = lyrics.Value.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

			var line = lines[new Random().Next(lines.Length)].Trim();

			receiver.Msg("♬♫ ..." + line + "... ♬♬");
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}
	}
}
