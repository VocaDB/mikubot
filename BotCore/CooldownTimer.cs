using System;
using System.Collections.Generic;

namespace MikuBot
{
	public class CooldownTimer
	{
		private readonly Dictionary<IrcName, DateTime> lastTimes;

		public CooldownTimer()
		{
			this.lastTimes = new Dictionary<IrcName, DateTime>();
		}

		/// <summary>
		/// Checks whether a specific IRC user or channel is allowed to perform an action, and if they are, updates the timer.
		/// </summary>
		/// <param name="name">IRC user or channel attempting to perform an action. This can be either a channel or an user.</param>
		/// <param name="sender">IRC user initiating the action. Used for replying to that user if necessary. This is usually an user, not a channel.</param>
		/// <param name="bot">Bot context. Cannot be null.</param>
		/// <param name="cooldownMs">Cooldown in milliseconds.</param>
		/// <param name="warn">Whether the user should be warned if the cooldown hasn't expired yet.</param>
		/// <returns>True if the user has access, meaning cooldown was expired.</returns>
		public bool CheckAccess(IrcName name, IrcName sender, IBotContext bot, int cooldownMs, bool warn)
		{
			cooldownMs = (int)(cooldownMs * bot.Config.CooldownMultiplier);

			if (cooldownMs <= 0)
				return true;

			if (!lastTimes.ContainsKey(name))
			{
				lastTimes.Add(name, DateTime.Now);
				return true;
			}

			var cooldown = TimeSpan.FromMilliseconds(cooldownMs);

			var time = lastTimes[name];
			var delay = DateTime.Now - time;

			if (delay < cooldown)
			{
				if (warn)
				{
					bot.Writer.Msg(sender, string.Format("Cooldown for this command hasn't expired yet. Try again in {0} second(s).",
						(int)((cooldown - delay).TotalSeconds)));
				}

				return false;
			}

			lastTimes[name] = DateTime.Now;
			return true;
		}

		/// <summary>
		/// Clears the cooldown for a specific user or channel.
		/// </summary>
		/// <param name="name">User or channel whose cooldown will be cleared.</param>
		public void Clear(IrcName name)
		{
			if (lastTimes.ContainsKey(name))
			{
				lastTimes.Remove(name);
			}
		}
	}
}
