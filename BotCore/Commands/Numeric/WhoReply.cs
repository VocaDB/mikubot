namespace MikuBot.Commands.Numeric
{
	/// <summary>
	/// RPL_WHOREPLY (352)
	/// </summary>
	public class WhoReply : NumericReply
	{
		public WhoReply(string prefix, string command, ParamCollection paramCollection)
			: base(prefix, command, paramCollection, (int)MikuBot.ReplyCode.RPL_WHOREPLY)
		{
		}

		public IrcName Channel
		{
			get
			{
				return new IrcName(ParamCollection.ParamOrEmpty(0));
			}
		}

		public Hostmask Hostmask
		{
			get
			{
				return new Hostmask(Nick, Ident, Hostname);
			}
		}

		public string Hostname
		{
			get
			{
				return ParamCollection.ParamOrEmpty(2);
			}
		}

		public string Ident
		{
			get
			{
				return ParamCollection.ParamOrEmpty(1);
			}
		}

		public IrcName Nick
		{
			get
			{
				return new IrcName(ParamCollection.ParamOrEmpty(4));
			}
		}
	}
}
