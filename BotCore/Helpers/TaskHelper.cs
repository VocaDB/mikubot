using System.Threading.Tasks;
using log4net;

namespace MikuBot.Helpers {

	public static class TaskHelper {

		private static readonly ILog log = LogManager.GetLogger(typeof(TaskHelper));

		public static void HandleTaskException(Task task) {

			log.Warn("Task caused an exception", task.Exception);

		}

	}
}
