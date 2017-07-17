using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuBot.DbModel.DataContracts;
using MikuBot.Modules;
using System.IO;
using MikuBot.Commands;

namespace MikuBot.DbPlugins {

	public class PrintLinks : MsgCommandModuleBase {

		private DbPluginsModuleFile modules;
		private static readonly string[] pictureExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };

		private void Print(TextWriter writer, IEnumerable<LinkRecordContract> records) {

			foreach (var record in records.OrderByDescending(m => m.Date)) {

				var linkCell = new StringBuilder();

				if (pictureExt.Any(e => e.Equals(Path.GetExtension(record.Url), StringComparison.InvariantCultureIgnoreCase)))
					linkCell.Append("<img width=\"50\" height=\"50\" src=\"" + record.Url + "\" />");

				linkCell.Append(string.Format("<a href='{0}'>{0}</a>", record.Url));

				writer.WriteLine(
					"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
					record.Date, record.Nick, linkCell.ToString(), record.Description);

			}

		}

		private void PrintFooter(TextWriter output) {

			output.WriteLine("</body>");
			output.WriteLine("</html>");

		}

		private void PrintHeader(TextWriter output, string chanName, int count) {

			output.WriteLine("<html>");
			output.WriteLine("<head><title>MikuBot links for {0}</title></head>", chanName);
			output.WriteLine("<body>");
			output.WriteLine("<h1>List of captured links</h1>");
			output.WriteLine("Generated {0}, {1} records total.<br />", DateTime.Now, count);
			output.WriteLine("<table border=\"0\" style=\"display: block;\">");
			output.WriteLine("<tr><th>Timestamp</th><th>Nick</th><th>URL</th><th>Line</th></tr>");

		}

		public override int BotCommandParamCount {
			get { return 2; }
		}

		public override string CommandDescription {
			get { return "Prints list of links to a HTML file."; }
		}

		public override string UsageHelp {
			get { return "PrintLinks <channel> <outfile>"; }
		}

		public override BotUserLevel MinUserLevel {
			get { return BotUserLevel.Admin; }
		}

		public override string Name {
			get { return "PrintLinks"; }
		}

		public override void HandleCommand(MsgCommand msg, IBotContext bot) {

			if (!CheckCall(msg, bot))
				return;

			var channel = new IrcName(msg.BotCommand.Params[0]);
			var outPath = msg.BotCommand.Params[1];

			var lines = modules.CommonServices.GetRecords(channel, null, 0, 500);

			using (var writer = new StreamWriter(outPath)) {

				PrintHeader(writer, channel.Name, lines.Length);

				Print(writer, lines);

				PrintFooter(writer);

			}

			var receiver = msg.Reply(bot.Writer);
			receiver.Msg("Link list written to " + outPath);

		}

		public override void OnLoaded(IBotContext bot, IModuleFile moduleFile) {
			modules = (DbPluginsModuleFile)moduleFile;
		}
	}

}
