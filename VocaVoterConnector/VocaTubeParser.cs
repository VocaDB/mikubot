using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Helpers;
using MikuBot.Modules;
using MikuBot.VocaDBConnector.Helpers;
using log4net;

namespace MikuBot.VocaDBConnector {

	public class VocaTubeParser : MsgCommandModuleBase {

		private static readonly ILog log = LogManager.GetLogger(typeof(VocaTubeParser));
		private readonly Regex regex = new Regex(@"http://.+\.vs");

		private VocaVoterConnectorFile connectorFile;

		private VocaTubeSong ParseSong(Stream input) {

			var ser = new DataContractJsonSerializer(typeof(VocaTubeSong));
			var obj = (VocaTubeSong)ser.ReadObject(input);

			return obj;

		}

		private void GetMediaData(Receiver receiver, string url) {

			log.Info("Requesting VocaTube info at " + url);
			var request = WebRequest.Create(url + ".json");
			request.Timeout = 5000;
			//request.Method = "HEAD";
			WebResponse response;

			try {
				response = request.GetResponse();
			} catch (WebException x) {
				log.Warn("VocaTube request error: " + x.Message);
				return;
			}

			var parsed = ParseSong(response.GetResponseStream());

			var title = parsed.title;

			if (string.IsNullOrEmpty(title)) {
				log.Info("Response contains no title");
				return;
			}

			var sb = new StringBuilder(Formatting.Bold + title + Formatting.Bold);

			var artist = parsed.artist;
			if (!string.IsNullOrEmpty(artist)) {
				sb.Append(" by " + artist);
			}

			var album = parsed.album;
			if (!string.IsNullOrEmpty(album)) {
				sb.Append(" on " + album);
			}

			var durationStr = response.Headers["X-Content-Duration"];
			int durationSec;
			int.TryParse(durationStr, out durationSec);

			if (durationSec > 0) {
				var time = TimeSpan.FromMilliseconds(durationSec);
				//sb.Append(", duration: " + time.ToString("g"));
				sb.Append(", duration: " + time.ToString(@"hh\:mm\:ss"));
			}

			var song = connectorFile.CallClient(client => client.GetSongDetailsByNameArtistAndAlbum(parsed.title, parsed.artist, parsed.album));

			if (song != null) {
				receiver.Msg(EntryFormattingHelper.FormatSongWithUrl(song.Song, connectorFile.Config));
			} else {
				receiver.Msg("VocaTube: " + sb);
			}

		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot) {

			if (cmd.BotCommand.Is("NoLink"))
				return;

			var receiver = new Receiver(bot.Writer, cmd.ChannelOrSenderNick);
			string url;

			if (cmd.BotCommand.Is(Name) && cmd.BotCommand.Params.HasParam(0)) {

				url = PluginHelper.MakeLink(cmd.BotCommand.Params.ParamOrEmpty(0), false);

			} else {

				var match = regex.Match(cmd.Text);

				if (!match.Success)
					return;

				url = match.Value;

			}

			Task.Factory.StartNew(() => GetMediaData(receiver, url))
				.ContinueWith(TaskHelper.HandleTaskException, TaskContinuationOptions.OnlyOnFaulted);

		}

		public override string HelpText {
			get { return "Parses VocaTube links. By prefixing the link with 'vocatube' the link is parsed even if it's not automatically recognized as a Vocatube link. By prefixing the link with 'nolink', all link parsing is skipped. This is useful if you don't want some link to be parsed."; }
		}

		public override bool IsPassive {
			get { return true; }
		}

		public override string Name {
			get { return "VocaTube"; }
		}

		public override string UsageHelp {
			get { return "[<vocatube>|<nolink>] <VocaTube URL>"; }
		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {

			connectorFile = (VocaVoterConnectorFile)moduleFile;

		}

	}

	[DataContract]
	public class VocaTubeSong {

		[DataMember]
		public string album { get; set; }

		[DataMember]
		public string artist { get; set; }

		[DataMember]
		public string title { get; set; }

	}

}
