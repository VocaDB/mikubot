using System;
using System.Configuration;
using System.Linq;

namespace MikuBot {

	public class Config : IConfig {

		private T GetSetting<T>(string name, Func<string, T> conv, T def) {
			var val = ConfigurationManager.AppSettings[name];
			return (!string.IsNullOrEmpty(val) ? conv(val) : def);
		}

		public string[] AuthKeys {
			get {
				return ConfigurationManager.AppSettings["AuthKeys"].Split(',');
			}
		}

		public IrcName[] AutoJoinChannels {
			get {
				return GetSetting("AutoJoinChannels", val => val.Split(','), new string[] {}).Select(c => new IrcName(c)).ToArray();
			}
		}

		public bool AutoLoadModules {
			get {
				return GetSetting("AutoLoadModules", val => bool.Parse(val), true);
			}
		}

		public double CooldownMultiplier {
			get {
				return GetSetting("CooldownMultiplier", double.Parse, 1.0);
			}
		}

		public string Culture {
			get {
				return GetSetting("Culture", val => val, "en-US");
			}
		}

		public string DanbooruPassHash {
			get {
				return GetSetting("DanbooruPassHash", val => val, null);
			}
		}

		public string DanbooruUserName {
			get {
				return GetSetting("DanbooruUserName", val => val, null);
			}
		}

		public string[] DisableModules {
			get {
				return GetSetting("DisableModules", val => val.Split(','), new string[] {} );
			}
		}

		public bool EnableServices {
			get {
				return GetSetting("EnableServices", val => bool.Parse(val), false);
			}
		}

		public int FloodPostExpirationTimeSeconds {
			get {
				return GetSetting("FloodPostExpirationTimeSeconds", val => int.Parse(val), 20);
			}
		}

		public int FloodPostIgnoreCount {
			get {
				return GetSetting("FloodPostIgnoreCount", val => int.Parse(val), 10);
			}
		}

		public int FloodPostIgnoreTimeMinutes {
			get {
				return GetSetting("FloodPostIgnoreTimeMinutes", val => int.Parse(val), 2);
			}
		}

		public string HelpLink {
			get {
				return GetSetting("HelpLink", s => s, null);
			}
		}

		public string HighlightShortcut {
			get {
				return GetSetting("HighlightShortcut", s => s, string.Empty);
			}
		}

		public string LogFile {
			get {
				return GetSetting("LogFile", s => s, "IrcLog.log");
			}
		}

		public string[] Modules {
			get {
				return ConfigurationManager.AppSettings["Modules"].Split(',');
			}
		}

		public string[] Nicks { 
			get {
				return ConfigurationManager.AppSettings["Nicks"].Split(',');	
			}
		}

		public string NickServPass {
			get {
				return ConfigurationManager.AppSettings["NickServPass"];
			}
		}

		public TimeSpan PingTimeout {
			get {
				return GetSetting("PingTimeout", val => TimeSpan.FromSeconds(int.Parse(val)), TimeSpan.FromMinutes(10));
			}
		}

		public int Port {
			get {
				return GetSetting("Port", val => int.Parse(val), 6667);
			}
		}

		public string Server {
			get {
				return ConfigurationManager.AppSettings["Server"];
			}
		}

		public string YoutubeApiKey {
			get {
				return ConfigurationManager.AppSettings["YoutubeApiKey"];
			}
		}

		public string GetString(string name) {
			return ConfigurationManager.AppSettings[name];
		}

	}

}
