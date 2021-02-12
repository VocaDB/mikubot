using System;

namespace MikuBot.Commands
{
	public class NumericReply : IrcCommand
	{
		public NumericReply(string prefix, string command, ParamCollection paramCollection, int replyCode)
			: base(prefix, command, paramCollection)
		{
			ReplyCode = replyCode;
		}

		public ReplyCode CommonReplyCode
		{
			get
			{
				return Enum.IsDefined(typeof(ReplyCode), ReplyCode) ? (ReplyCode)ReplyCode : MikuBot.ReplyCode.Unknown;
			}
		}

		public int ReplyCode { get; private set; }
	}
}
