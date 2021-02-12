using log4net;

namespace MikuBot
{
	public class LogReceiver : IReceiver
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(LogReceiver));

		public void Msg(string text)
		{
			log.Info("MSG: " + text);
		}

		public void Notice(string text)
		{
			log.Info("NOTICE: " + text);
		}
	}
}
