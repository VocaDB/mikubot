using System;
using System.Collections.Generic;
using System.Linq;
using MikuBot.Commands;

namespace MikuBot.ExtraPlugins.Helpers {

	public class MessageBuffer {

		private const int bufferLength = 100;
		private readonly Dictionary<IrcName, Queue<BufferLine>> lines = new Dictionary<IrcName, Queue<BufferLine>>();

		public IEnumerable<BufferLine> GetLines(IrcName channel) {

			return GetLines(channel, bufferLength);

		}

		public IEnumerable<BufferLine> GetLines(IrcName channel, int maxLines) {

			if (!lines.ContainsKey(channel))
				return new BufferLine[] { };

			var linesForChan = lines[channel];

			var buf = linesForChan.Skip(Math.Max(linesForChan.Count - maxLines, 0)).Take(maxLines);

			return buf;

		}

		public void Record(IrcName channel, MsgCommand line) {

			if (!channel.IsChannel)
				return;

			if (!lines.ContainsKey(channel))
				lines.Add(channel, new Queue<BufferLine>());

			var chan = lines[channel];

			chan.Enqueue(new BufferLine(line));

			if (chan.Count > bufferLength)
				chan.Dequeue();

		}

	}

	public class BufferLine {

		public BufferLine(MsgCommand message) {
			Message = message;
			Timestamp = DateTime.Now;
		}

		public MsgCommand Message { get; private set; }

		public DateTime Timestamp { get; private set; }

	}


}
