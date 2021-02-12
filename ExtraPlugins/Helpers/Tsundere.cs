using System;
using System.Collections.Specialized;

namespace MikuBot.ExtraPlugins.Helpers
{
	public static class Tsundere
	{
		private static readonly StringCollection tsun = new StringCollection {
			"I-it's not like I wanted to.",
			"I-it's not like I wanted t-to.",
			"I-it's not like I w-wanted to.",
			"I-I'm only doing this b-because you ordered me to.",
			"I-I'm only doing this because you ordered me to.",
			"I-I might not agree to to your request e-every time.",
			"I-I think you're doing this too often.",
			"I-I'll make sure this feature is d-disabled in the next Service Pack."
		};

		public static void Tsun(Receiver receiver)
		{
			ParamIs.NotNull(() => receiver);

			if (new Random().Next(100) > 50)
				receiver.Msg(BotHelper.ChooseRandom(tsun));
		}
	}
}
