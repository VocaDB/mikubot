namespace MikuBot
{
	/// <summary>
	/// http://www.irchelp.org/irchelp/rfc/chapter6.html
	/// </summary>
	public enum ReplyCode
	{
		Unknown = 0,

		/// <summary>
		/// WHOIS user reply
		/// </summary>
		RPL_WHOISUSER = 311,

		/// <summary>
		/// End of WHO list
		/// </summary>
		RPL_ENDOFWHO = 315,

		/// <summary>
		/// WHOIS channels list
		/// </summary>
		RPL_WHOISCHANNELS = 319,

		/// <summary>
		/// WHO list
		/// </summary>
		RPL_WHOREPLY = 352,

		/// <summary>
		/// NAMES list
		/// </summary>
		RPL_NAMREPLY = 353,

		/// <summary>
		/// End of NAMES list
		/// </summary>
		RPL_ENDOFNAMES = 366,

		/// <summary>
		/// End of MOTD
		/// </summary>
		RPL_ENDOFMOTD = 376,

		/// <summary>
		/// No MOTD
		/// </summary>
		ERR_NOMOTD = 422,

		ERR_NICKNAMEINUSE = 433,

		ERR_NICKCOLLISION = 436
	}
}
