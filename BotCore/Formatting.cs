using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikuBot {

	public static class Formatting {

		public const char Bold = '\u0002';

		public const char Color = '\u0003';

		public const char Normal = '\u000F';

		public const char Underline = '\u001F';

	}

	public class ColorCode {

		private readonly string code;

		public static implicit operator ColorCode(string code) {
			return new ColorCode(code);
		}

		public static readonly ColorCode Blue = "02";
		public static readonly ColorCode Green = "03";
		public static readonly ColorCode Red = "05";

		public ColorCode(string code) {
			this.code = code;
		}

		public string Format {
			get { return Formatting.Color + code; }
		}

		public override string ToString() {
			return Format;
		}

	}

}
