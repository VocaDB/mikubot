using System;
using VocaDb.Model.Service;

namespace MikuBot.VocaDBConnector.Helpers
{
	public class SearchHelper
	{
		public static string GetSearchQuery(BotCommand botCommand, ref NameMatchMode nameMatchMode, bool allowExactByQuotes = true)
		{
			if (botCommand.Params.ParamOrEmpty(0).Equals("exact", StringComparison.InvariantCultureIgnoreCase))
			{
				nameMatchMode = NameMatchMode.Exact;
				var reader = new CmdReader(botCommand.CommandString);
				reader.ReadNext();
				return reader.ReadToEnd();
			}
			else if (allowExactByQuotes && botCommand.CommandString.StartsWith("\"") && botCommand.CommandString.EndsWith("\""))
			{
				nameMatchMode = NameMatchMode.Exact;
				return botCommand.CommandString.Substring(1, botCommand.CommandString.Length - 2);
			}
			else
			{
				return botCommand.CommandString;
			}
		}
	}
}
