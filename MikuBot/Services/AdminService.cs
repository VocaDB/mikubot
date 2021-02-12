using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MikuBot.Services
{
	[ServiceContract]
	public class AdminService
	{
		private Bot Bot
		{
			get
			{
				return Program.Bot;
			}
		}

		[OperationContract]
		public void Reconnect()
		{
			Bot.Reconnect();
		}
	}
}
