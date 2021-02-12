using System.Text.RegularExpressions;

namespace MikuBot.Helpers
{
	public static class HtmlUtils
	{
		public static string StripHTML(string htmlString)
		{
			//This pattern Matches everything found inside html tags;
			//(.|\n) - > Look for any character or a new line
			// *?  -> 0 or more occurences, and make a non-greedy search meaning
			//That the match will stop at the first available '>' it sees, and not at the last one
			//(if it stopped at the last one we could have overlooked
			//nested HTML tags inside a bigger HTML tag..)
			// Thanks to Oisin and Hugh Brown for helping on this one...

			return Regex.Replace(htmlString, @"<(.|\n)*?>", string.Empty);
		}
	}
}
