using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MikuBot.Site.Helpers {

	/// <summary>
	/// Copied from VocaDB
	/// </summary>
	public static class WebHelper {

		private static readonly Regex regex = new Regex(@"http[s]?\:[a-zA-Z0-9_\#\-\.\:\/\%\?\&\=\+\(\)\!]+");

		private static bool IsFullLink(string str) {

			return (str.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase)
				|| str.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
				|| str.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase));

		}

		/// <summary>
		/// Makes a proper URL from a possible URL without a http:// prefix.
		/// </summary>
		/// <param name="partialLink">Partial URL. Can be null.</param>
		/// <param name="assumeWww">Whether to assume the URL should start with www.</param>
		/// <returns>Full URL including http://. Can be null if source was null.</returns>
		public static string MakeLink(string partialLink, bool assumeWww = false) {

			if (string.IsNullOrEmpty(partialLink))
				return partialLink;

			if (IsFullLink(partialLink))
				return partialLink;

			if (assumeWww && !partialLink.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
				return string.Format("http://www.{0}", partialLink);

			return string.Format("http://{0}", partialLink);

		}

		public static string ReplaceUrisWithLinks(string text) {

			var parsed = new StringBuilder(text);

			var matches = regex.Matches(text);

			var indexOffset = 0;

			foreach (Match match in matches) {

				var link = GetLink(match.Value);

				if (link != match.Value) {

					parsed.Replace(match.Value, link, match.Index + indexOffset, match.Length);

					indexOffset += (link.Length - match.Value.Length);

				}
			}

			return parsed.ToString();

		}

		public static string GetLink(string text) {

			Uri uri;

			if (Uri.TryCreate(text, UriKind.Absolute, out uri)) {
				return string.Format("<a class='extLink' href='{0}'>{0}</a>", text);
			} else {
				return text;
			}


		}

	}

}