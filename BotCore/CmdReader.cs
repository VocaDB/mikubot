using System;

namespace MikuBot {

	public class CmdReader {

		private int index;
		private readonly string input;

		private void ScrollToNextNonSpace() {

			while (InBounds && input[index] == ' ')
				index++;
	
		}

		public CmdReader(string input)
			: this(input, 0) {
			this.input = input;
		}

		public CmdReader(string input, int index) {
			this.input = input ?? string.Empty;
			this.index = Math.Max(index, 0);
		}

		public bool InBounds {
			get {
				return (index < input.Length);
			}
		}

		public char Peek {
			get {
				ScrollToNextNonSpace();
				return (InBounds ? input[index] : '\0');
			}
		}

		public string ReadNext() {

			ScrollToNextNonSpace();

			if (!InBounds)
				return string.Empty;

			var end = input.IndexOf(' ', index);

			if (end == -1)
				end = input.Length;

			var text = input.Substring(index, end - index);
			index = end + 1;

			return text;

		}

		public string ReadToEnd() {

			ScrollToNextNonSpace();

			if (!InBounds)
				return string.Empty;

			var	end = input.Length;

			var text = input.Substring(index, end - index);
			index = end + 1;

			return text;

		}

	}
}
