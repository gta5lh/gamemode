using System;
using System.Text;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using EasyNetQ;

namespace Gamemode.Controllers
{
	public class DonationController : Script
	{
		[Queue("deposit.made", ExchangeName = "eventbus")]
		public class MyMessage
		{
			public long Amount { get; set; }
			public long PlayerID { get; set; }
		}

		[ServerEvent(Event.ResourceStartEx)]
		private async void ResourceStartEx(string resourceName)
		{
			var bus = RabbitHutch.CreateBus("host=localhost");

			await bus.PubSub.SubscribeAsync<MyMessage>(
				string.Empty, msg =>
				{
					NAPI.Task.Run(() =>
					{
						CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(msg.PlayerID);
						if (targetPlayer == null)
						{
							return;
						}

						NAPI.ClientEvent.TriggerClientEvent(targetPlayer, "DisplayDonationNotification", msg.Amount);
					});
				}
			);
		}
	}
}
