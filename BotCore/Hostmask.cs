using System;
using System.Text.RegularExpressions;

namespace MikuBot {

	/// <summary>
	/// Hostmask is the combination of nickname, ident and hostname.
	/// </summary>
	public struct Hostmask : IEquatable<Hostmask>, IEquatable<string> {

		private static readonly Regex hostmaskRegex = new Regex(@"(\S+)\!(\S+)@(\S+)");

		public static Hostmask Empty {
			get {
				return new Hostmask(string.Empty);
			}
		}

		public static bool IsValidHostmask(string hostmask) {

			ParamIs.NotNull(() => hostmask);

			return hostmaskRegex.IsMatch(hostmask);

		}

		private readonly string hostmask;
		private Match match;

		private Match Match {
			get {
				return match ?? (match = hostmaskRegex.Match(hostmask));
			}
		}

		public Hostmask(string hostmask) {

			ParamIs.NotNull(() => hostmask);

			this.hostmask = hostmask;
			this.match = null;

		}

		public Hostmask(IrcName nick, string ident, string hostname) {

			hostmask = string.Format("{0}!{1}@{2}", nick, ident, hostname);
			this.match = null;

		}

		public string Hostname {
			get {
				return (Match.Success ? Match.Groups[3].Value : hostmask);
			}
		}

		public string Ident {
			get {
				return (Match.Success ? Match.Groups[2].Value : hostmask);
			}
		}

		public bool IsValid {
			get {
				return Match.Success;
			}
		}

		public IrcName Nick {
			get {
				return new IrcName(Match.Success ? Match.Groups[1].Value : hostmask);
			}
		}

		public bool Equals(Hostmask another) {
			return another.hostmask.Equals(hostmask, StringComparison.InvariantCultureIgnoreCase);
		}

		public bool Equals(string another) {
			return hostmask.Equals(another, StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool Equals(object obj) {
			return (obj is Hostmask && Equals((Hostmask)obj));
		}

		public override int GetHashCode() {
			return hostmask.GetHashCode();
		}

		public override string ToString() {
			return hostmask;
		}

		public static bool operator ==(Hostmask first, Hostmask second) {
			return first.Equals(second);
		}

		public static bool operator !=(Hostmask first, Hostmask second) {
			return !first.Equals(second);
		}	

	}
}
