using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikuBot.LogReaders {

	public class IrcLogLine {

		public DateTime Date { get; private set; }

		public string Nick { get; private set; }

		public string Text { get; private set; }

	}

}
