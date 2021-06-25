using System;
using Gamemode.Cache.GangZone;
using Gamemode.Commands.Admin;
using Gamemode.Controllers;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Test
{
	public class TestCommand : Script
	{
		[Command("tmoney", "/tmoney", Alias = "tm", GreedyArg = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void Money(CustomPlayer player, string message = null)
		{
			player.SendChatMessage($"{player.Money}");
		}

		[Command("tlogin", "/tlogin", Alias = "tl", GreedyArg = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void Login(CustomPlayer player, string message = null)
		{
			player.LoggedInAt = DateTime.UtcNow;
			player.SendChatMessage($"{player.LoggedInAt}");
		}

		[Command("rgz")]
		[AdminMiddleware(AdminRank.Owner)]
		public async void Rb(CustomPlayer player)
		{
			var zones = await GangZoneCache.LoadZones();
			NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RenderGangZones", zones);
		}

		[Command("rgz")]
		[AdminMiddleware(AdminRank.Owner)]
		public async void Sgw(CustomPlayer player)
		{
			var zones = await GangZoneCache.LoadZones();
			NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RenderGangZones", zones);
		}

		[Command("igw")]
		[AdminMiddleware(AdminRank.Owner)]
		public async void InitGangWar(CustomPlayer player)
		{
			await Services.GangWarService.InitGangWar();
		}

		[Command("sgw")]
		[AdminMiddleware(AdminRank.Owner)]
		public async void StartGangWar(CustomPlayer player, string? minutesInput = null)
		{
			if (minutesInput == null)
			{
				minutesInput = "15";
			}

			short minutes;

			try
			{
				minutes = short.Parse(minutesInput);
			}
			catch (Exception)
			{
				return;
			}


			DateTime finishTime = DateTime.UtcNow.AddMinutes(minutes);
			await Services.GangWarService.StartGangWar(finishTime);
		}


		[Command("fgw")]
		[AdminMiddleware(AdminRank.Owner)]
		public async void FinishGangWar(CustomPlayer player)
		{
			await Services.GangWarService.FinishGangWar();
		}
	}
}
