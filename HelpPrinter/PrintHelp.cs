using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using MikuBot.AdminModules.Helpers;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.AdminModules {

	public class PrintHelp : MsgCommandModuleBase {

		private string FormatHelpText(string text) {

			return HttpUtility.HtmlEncode(text.Replace(Formatting.Bold, ' ')).Replace("\n", "<br />");

		}

		private void Print(TextWriter writer, IEnumerable<IModule> modules) {

			foreach (var module in modules.Where(m => m.MinUserLevel <= BotUserLevel.Identified).OrderBy(m => m.Name)) {
				
				writer.WriteLine(string.Format(
					"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", 
					module.Name, FormatHelpText(module.HelpText), module.MinUserLevel == BotUserLevel.Unidentified ? "No" : "Yes"));

			}

		}

		private void PrintFooter(TextWriter output) {
			
			output.WriteLine("</body>");
			output.WriteLine("</html>");

		}

		private void PrintHeader(TextWriter output) {
			
			output.WriteLine("<html>");
			output.WriteLine("<head><title>MikuBot help</title></head>");
			output.WriteLine("<body>");
			output.WriteLine("<h1>Help file for MikuBot v" + Application.ProductVersion + "</h1>");
			output.WriteLine("Generated " + DateTime.Now);

			output.WriteLine("<h2>Overview</h2>");
			output.WriteLine(ResourceHelper.ReadTextFile("Overview.html"));

			output.WriteLine("<h2>Calling the bot</h2>");
			output.WriteLine(ResourceHelper.ReadTextFile("CallingBot.html"));

			output.WriteLine("<h2>User accounts</h2>");
			output.WriteLine(ResourceHelper.ReadTextFile("Accounts.html"));

			output.WriteLine("<h2>Command listing</h2>");
			output.WriteLine("<table border=\"0\">");
			output.WriteLine("<tr><th>Module</th><th>Description</th><th>Requires registration</th></tr>");

		}

		private void PrintTitleRow(TextWriter writer, string title) {

			writer.WriteLine("<tr><td colspan=\"3\"><h3>" + title + "</h3></td></tr>");

		}

		public override int BotCommandParamCount {
			get { return 1; }
		}

		public override string CommandDescription {
			get { return "Prints all help text to a HTML file."; }
		}

		public override string UsageHelp {
			get { return "PrintHelp <outfile>"; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "PrintHelp"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var outPath = msg.BotCommand.Params[0];

			using (var writer = new StreamWriter(outPath)) {

				PrintHeader(writer);

				var activeModules = bot.ModuleManager.BuiltinModules.Concat(bot.ModuleManager.MsgCommandModules.Where(m => !m.IsPassive));

				var passive = bot.ModuleManager.GenericModules
					.Concat(bot.ModuleManager.MsgCommandModules
						.Where(m => m.IsPassive).Cast<IModule>());

				PrintTitleRow(writer, "Module commands");
				Print(writer, activeModules);

				PrintTitleRow(writer, "Passive modules");
				Print(writer, passive);

				PrintFooter(writer);

			}

			var receiver = msg.Reply(bot.Writer);
			receiver.Msg("Help file written to " + outPath);

		}

	}

}
