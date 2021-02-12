using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MikuBot.DbModel.DataContracts;
using MikuBot.DbModel.Services;

namespace MikuBot.DbPlugins.Utils
{
	public class LogFileLinkReader
	{
		private readonly string logFilePath;
		private readonly CommonServices services;

		private void ReadLine(string line)
		{
			var nickRegex = new Regex(@"\<[a-zA-Z]+\>");
			var nickMatch = nickRegex.Match(line);

			if (!nickMatch.Success || nickMatch.Index + nickMatch.Length + 1 >= line.Length)
				return;

			var nick = new IrcName(nickMatch.Value.Substring(1, nickMatch.Length - 2));

			DateTime date;

			var dateStr = line.Substring(0, nickMatch.Index - 1);

			if (!DateTime.TryParse(dateStr, out date))
				date = DateTime.Now;

			var text = line.Substring(nickMatch.Index + nickMatch.Length + 1).Trim();

			var uris = BotHelper.GetUris(text).Select(u => u.AbsoluteUri).ToArray();

			if (!uris.Any())
				return;

			var channel = new IrcName("#mikuchan");
			var contracts = uris.Select(u => new LinkRecordContract(u, nick, channel, text)).ToArray();

			services.RecordLinks(contracts, channel);
		}

		public LogFileLinkReader(CommonServices services, string logFilePath)
		{
			this.services = services;
			this.logFilePath = logFilePath;
		}

		public void ReadLinks()
		{
			using (var reader = new StreamReader(logFilePath))
			{
				string line;

				while ((line = reader.ReadLine()) != null)
				{
					ReadLine(line);
				}
			}
		}
	}
}
