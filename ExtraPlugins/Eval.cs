using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Eval : MsgCommandModuleBase
	{
		public override int BotCommandParamCount
		{
			get { return 1; }
		}

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			if (!CheckCall(cmd, bot))
				return;

			var reply = cmd.Reply(bot.Writer);
			var script = cmd.BotCommand.CommandString;
			var compiler = new CSharpCodeProvider().CreateCompiler();

			var par = new CompilerParameters();

			// *** Start by adding any referenced assemblies
			par.ReferencedAssemblies.Add("System.dll");
			//par.ReferencedAssemblies.Add();

			par.GenerateInMemory = true;

			var result = compiler.CompileAssemblyFromSource(par, script);

			if (result.Errors.HasErrors)
			{
				reply.Msg("Error: " + result.Errors[0].ErrorText);
				return;
			}

			var compiled = result.CompiledAssembly;
		}

		public override string CommandDescription
		{
			get { return "Executes a line of C# code."; }
		}

		public override InitialModuleStatus InitialStatus
		{
			// TODO: remove when finished
			get { return InitialModuleStatus.NotLoaded; }
		}

		public override string UsageHelp
		{
			get { return "eval <code>"; }
		}

		public override BotUserLevel MinUserLevel
		{
			get { return BotUserLevel.Admin; }
		}

		public override string Name
		{
			get { return "Eval"; }
		}
	}
}
