namespace MikuBot.Commands {

	/// <summary>
	/// KILL message
	/// </summary>
	public class KillMessage : IrcCommand {

		public KillMessage(string prefix, ParamCollection paramCollection)
			: base(prefix, "KILL", paramCollection) {

		}

		public string Comment {
			get {
				return ParamCollection.ParamOrEmpty(1);
			}
		}

		public IrcName KilledNick {
			get {
				return new IrcName(ParamCollection.ParamOrEmpty(0));
			}
		}

	}

}
