using System;
using System.Text;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using EasyNetQ;
using Rpc.Eventbus;
using server.Rabbit;
using Gamemode.Services.Player;

namespace Gamemode.Controllers
{
	public class EventbusController : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private async void ResourceStartEx(string resourceName)
		{
			string? rabbitMQURL = System.Environment.GetEnvironmentVariable("RABBIT_MQ_URL");
			if (rabbitMQURL == null)
			{
				rabbitMQURL = "host=localhost";
			}

			var bus = RabbitHutch.CreateBus(rabbitMQURL, sr => sr.Register(typeof(ITypeNameSerializer), new Serializer()));

			await bus.PubSub.SubscribeAsync<DepositMadeEvent>(
				string.Empty, msg => DonationService.DisplayDonationNotification(msg.PlayerID, msg.Amount), x => x.WithTopic("deposit.made")
			);
		}
	}
}
