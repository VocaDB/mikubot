using System;

namespace MikuBot
{
	/// <summary>
	/// Name of a channel or the nickname of a user.
	/// </summary>
	public struct IrcName : IEquatable<IrcName>, IEquatable<string>
	{
		private readonly string original;
		private readonly string lc;

		public static IrcName Empty
		{
			get
			{
				return new IrcName();
			}
		}

		public static string ParseNick(string hostname)
		{
			ParamIs.NotNull(() => hostname);

			var expPos = hostname.IndexOf('!');
			return (expPos != -1 ? hostname.Substring(0, expPos) : hostname);
		}

		public static IrcName ParseFromHost(string hostname)
		{
			return new IrcName(ParseNick(hostname));
		}

		public static bool operator ==(IrcName first, IrcName second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(IrcName first, IrcName second)
		{
			return !first.Equals(second);
		}

		public IrcName(string name)
			: this()
		{
			ParamIs.NotNull(() => name);

			original = name;
			lc = name.ToLowerInvariant();
		}

		public bool IsChannel
		{
			get
			{
				return (Name.StartsWith("#") || Name.StartsWith("&"));
			}
		}

		public bool IsEmpty
		{
			get
			{
				return Name == string.Empty;
			}
		}

		public string LowercaseName
		{
			get { return lc; }
		}

		public string Name
		{
			get { return original; }
		}

		public bool Equals(string name)
		{
			return (Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public bool Equals(IrcName other)
		{
			return (other.LowercaseName.Equals(lc));
		}

		public override int GetHashCode()
		{
			return LowercaseName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IrcName))
				return false;

			return Equals((IrcName)obj);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
