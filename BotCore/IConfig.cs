namespace MikuBot {

	public interface IConfig {

		double CooldownMultiplier { get; }

		string DanbooruPassHash { get; }

		string DanbooruUserName { get; }

		int FloodPostExpirationTimeSeconds { get; }

		int FloodPostIgnoreCount { get; }

		int FloodPostIgnoreTimeMinutes { get; }

		string HelpLink { get; }

		string LogFile { get; }

		string NickServPass { get; }

		string YoutubeApiKey { get; }

		// TODO: certain properties (like nickserv pass) should be protected
		string GetString(string name);

	}

}
