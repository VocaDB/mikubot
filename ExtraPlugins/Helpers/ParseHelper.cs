using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MikuBot.ExtraPlugins.Helpers {

	public static class ParseHelper {

		private static readonly ILog log = LogManager.GetLogger(typeof (ParseHelper));

		public static Encoding GetEncoding(string encodingStr) {

			if (string.IsNullOrEmpty(encodingStr))
				return Encoding.UTF8;

			try {
				return Encoding.GetEncoding(encodingStr);
			} catch (ArgumentException x) {
				log.Debug("Unable to get encoding", x);
				return Encoding.UTF8;
			}

		}

		public static Stream GetStream(Stream stream, string encStr) {

			if (encStr.ToLower().Contains("gzip"))
				stream = new GZipStream(stream, CompressionMode.Decompress);
			else if (encStr.ToLower().Contains("deflate"))
				stream = new DeflateStream(stream, CompressionMode.Decompress);

			return stream;

		}

	}

}
