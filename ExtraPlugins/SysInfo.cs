using System;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class SysInfo : MsgCommandModuleBase
	{
		private string CollapseWhitespace(string str)
		{
			var res = Regex.Replace(str, @"\s{2,}", " ");

			return res;
		}

		private string MemoryString
		{
			get
			{
				var mgmt = new ManagementClass("Win32_ComputerSystem");
				var obj = mgmt.GetInstances().Cast<ManagementObject>().FirstOrDefault();

				if (obj == null)
					return "Unknown memory";

				//var mobo = obj.Properties["Manufacturer"].Value + " " + obj.Properties["Model"].Value;

				var compInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
				var totalRam = long.Parse(compInfo.TotalPhysicalMemory.ToString()) / 1024 / 1024;
				var freeRam = long.Parse(compInfo.AvailablePhysicalMemory.ToString()) / 1024 / 1024;

				return "RAM: " + freeRam + "MB free, " + totalRam + " MB total";
			}
		}

		private string ProcString
		{
			get
			{
				var procCount = Environment.ProcessorCount;

				var procInfo = "Unknown processor";

				var mgmt = new ManagementClass("Win32_Processor");

				var objCol = mgmt.GetInstances().Cast<ManagementObject>().FirstOrDefault();

				if (objCol != null)
				{
					procInfo = CollapseWhitespace(objCol.Properties["Name"].Value.ToString());
				}

				return procCount + "x " + procInfo;
			}
		}

		public override int CooldownChannelMs
		{
			get { return 10000; }
		}

		public override int CooldownUserMs
		{
			get { return 30000; }
		}

		public override string CommandDescription
		{
			get { return "Displays information about the environement this bot is running in."; }
		}

		public override string Name
		{
			get { return "SysInfo"; }
		}

		public override void HandleCommand(MsgCommand chat, IBotContext bot)
		{
			if (!CheckCall(chat, bot))
				return;

			var os = Environment.OSVersion.ToString();
			var clrVer = Environment.Version.ToString();
			var arch = Environment.Is64BitOperatingSystem ? "x86-64" : "x86";

			bot.Writer.Msg(chat.ChannelOrSenderNick,
				os + " " + arch
				+ " | CLR " + clrVer
				+ " | " + ProcString
				+ " | " + MemoryString);
		}
	}
}
