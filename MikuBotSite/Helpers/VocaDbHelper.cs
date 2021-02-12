using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MikuBot.Site.Helpers
{
	public class VocaDbHelper
	{
		private static readonly Regex[] linkMatchers = new[] {
			new Regex(@"vocadb.net/(Song)/Details/(\w+)"),
			new Regex(@"vocadb.net/(S)/(\w+)"),
		};

		public static string GetVocaDbPreviewUrl(UrlHelper urlHelper, string linkUrl)
		{
			var matcher = linkMatchers.FirstOrDefault(m => m.IsMatch(linkUrl));

			if (matcher == null)
				return null;

			var match = matcher.Match(linkUrl);
			var grp = match.Groups[1].Value.ToLowerInvariant();
			var id = match.Groups[2].Value;

			if (grp == "s")
				grp = "song";

			switch (grp)
			{
				case "song":
					return urlHelper.Action("PreviewSong", "VocaDb", new { id });
			}

			return null;
		}
	}
}