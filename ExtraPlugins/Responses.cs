using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MikuBot.Commands;
using MikuBot.Modules;

namespace MikuBot.ExtraPlugins
{
	public class Responses : MsgCommandModuleBase
	{
		public override string Name => "Responses";

		public override void HandleCommand(MsgCommand cmd, IBotContext bot)
		{
			var match = Regex.Match(cmd.Text, @"^(\w+)\s+" + bot.OwnNick + "$", RegexOptions.IgnoreCase);

			if (match.Success)
			{
			}
		}
	}
}
