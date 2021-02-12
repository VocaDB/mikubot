namespace MikuBot.Commands
{
	/// <summary>
	/// PING command
	/// </summary>
	public class PingCommand : IrcCommand
	{
		public PingCommand(string prefix, ParamCollection paramCollection)
			: base(prefix, "PING", paramCollection)
		{
		}

		public string Origin
		{
			get
			{
				return ParamCollection.Trailing;
			}
		}
	}
}
