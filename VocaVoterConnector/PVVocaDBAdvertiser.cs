using System.Linq;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using VocaDb.Model.DataContracts.Songs;
using VocaDb.Model.Domain.PVs;

namespace MikuBot.VocaDBConnector
{
	public class PVVocaDBAdvertiser : MsgCommandModuleBase
	{
		private VocaVoterConnectorFile connectorFile;

		private async Task<SongWithAlbumContract> CallVocaDbAsync(PVService service, string pvId, ClientType clientType)
		{
			return await connectorFile.CallClientAsync(client => client.GetSongWithPVAsync(service, pvId), clientType);
		}

		private async Task<NicoApi.VideoDataResult> CallNicoAsync(string pvId)
		{
			return await Task.Run(() => NicoApi.VideoApiClient.GetVideoData(pvId, true));
		}

		private async Task GetPvInfoAsync(Receiver receiver, IBotContext bot, PVService service, string pvId)
		{
			var vocaDbTask = CallVocaDbAsync(service, pvId, ClientType.VocaDb);
			var utaiteDbTask = CallVocaDbAsync(service, pvId, ClientType.UtaiteDb);

			NicoApi.VideoDataResult data = null;
			if (service == PVService.NicoNicoDouga)
			{
				data = await CallNicoAsync(pvId);
			}

			var songVocaDb = await vocaDbTask;
			var songUtaiteDb = await utaiteDbTask;
			var song = songVocaDb ?? songUtaiteDb;
			var site = songVocaDb != null ? ClientType.VocaDb : ClientType.UtaiteDb;

			if (data != null)
			{
				if (song != null)
				{
					receiver.Msg(EntryFormattingHelper.FormatSongWithAlbumAndUrl(song, data, connectorFile.Config, site));
				}
				else
				{
					receiver.Msg(string.Format("NicoVideo: {0}{1}{0} at {2} by {3}, {4} views",
						Formatting.Bold, data.Title, data.Created.ToString("g"), data.Author, data.Views));
				}
			}
			else if (song != null)
			{
				receiver.Msg(EntryFormattingHelper.FormatSongWithAlbumAndUrl(song, connectorFile.Config, site));
			}
			else if (service == PVService.Youtube)
			{
				var youtubeModule = bot.ModuleManager.FindModule("Youtube");

				if (youtubeModule != null && !bot.ModuleManager.IsModuleEnabled(youtubeModule))
					bot.ModuleManager.CallModule(bot, youtubeModule);
			}
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (cmd.BotCommand.Is("NoLink"))
				return;

			var possibleUrl = cmd.Text;
			var matcher = VideoService.Services.FirstOrDefault(m => m.IsMatch(possibleUrl));

			if (matcher == null)
				return;

			var receiver = cmd.Reply(bot.Writer);
			var pvId = matcher.GetId(possibleUrl);
			Task.Run(() => GetPvInfoAsync(receiver, bot, matcher.Service, pvId));
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile)
		{
			connectorFile = (VocaVoterConnectorFile)moduleFile;
		}

		public override string HelpText
		{
			get
			{
				return "Retrieves video information from VocaDB instead of Youtube/NND when the PV has been added to VocaDB";
			}
		}

		public override bool IsPassive
		{
			get { return true; }
		}

		public override string Name
		{
			get { return "PVVocaDBAdvertiser"; }
		}
	}
}
