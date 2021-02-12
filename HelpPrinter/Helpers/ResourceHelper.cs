using System.IO;
using System.Reflection;

namespace MikuBot.AdminModules.Helpers
{
	public static class ResourceHelper
	{
		public static string ReadTextFile(string fileName)
		{
			var asm = typeof(ResourceHelper).Assembly;
			var s = asm.GetManifestResourceNames();
			using (var stream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources." + fileName))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
