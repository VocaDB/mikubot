using System.Linq;
using MikuBot.Commands;
using MikuBot.DbModel.DataContracts;
using MikuBot.Modules;

namespace MikuBot.DbPlugins
{
	public class LinkRecorder : MsgCommandModuleBase
	{
		private DbPluginsModuleFile modules;

		private void RecordLinks(string[] uris, IrcName nick, IrcName channel, string line)
		{
			var contracts = uris.Select(u => new LinkRecordContract(u, nick, channel, line)).ToArray();

			modules.CommonServices.RecordLinks(contracts, channel);
		}

		public override string HelpText
		{
			get { return "Records links posted on the channel."; }
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "RecordLink"; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (cmd.BotCommand.Is("link"))
				return;

			var text = cmd.Text;

			var uris = BotHelper.GetUris(text).Select(u => u.AbsoluteUri).ToArray();

			if (!uris.Any())
				return;

			var nick = cmd.Sender.Nick;

			RecordLinks(uris, nick, cmd.ChannelOrSenderNick, text);

			//Task.Factory.StartNew(() => RecordLinks(uris, cmd.Sender.Nick))
			//	.ContinueWith(BotHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);	
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			modules = (DbPluginsModuleFile)moduleFile;
		}
	}
}
