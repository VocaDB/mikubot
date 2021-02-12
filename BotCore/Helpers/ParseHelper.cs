namespace MikuBot.Helpers
{
	public static class ParseHelper
	{
		public static int ParseIntOrDefault(string str, int def)
		{
			if (string.IsNullOrEmpty(str))
				return def;

			int val;
			if (!int.TryParse(str, out val))
				return def;

			return val;
		}
	}
}
