using System;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
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
		[Command("tmoney", "/tmoney", Alias = "tm", GreedyArg = true, SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void Money(CustomPlayer player, string message = null)
		{
			player.SendChatMessage($"{player.Money}");
		}

		[Command("tlogin", "/tlogin", Alias = "tl", GreedyArg = true, SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void Login(CustomPlayer player, string message = null)
		{
			player.LoggedInAt = DateTime.UtcNow;
			player.SendChatMessage($"{player.LoggedInAt}");
		}

		[Command("rgz", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void Rb(CustomPlayer player)
		{
			var zones = await GangZoneCache.LoadZones();
			NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RenderGangZones", zones);
		}

		[Command("igw", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void InitGangWar(CustomPlayer player)
		{
			await Services.GangWarService.InitGangWar();
		}

		[Command("sgw", SensitiveInfo = true, Hide = true)]
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


		[Command("fgw", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void FinishGangWar(CustomPlayer player)
		{
			await Services.GangWarService.FinishGangWar();
		}

		[Command("afg", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void AddGangWarKill(CustomPlayer player, string fractionIdInput = null, string amountInput = null)
		{
			if (fractionIdInput == null || amountInput == null) return;
			if (!GangWarCache.IsInProgress()) return;

			byte fractionId;
			short amount;

			try
			{
				fractionId = byte.Parse(fractionIdInput);
				amount = short.Parse(amountInput);
			}
			catch
			{
				return;
			}

			GangWarCache.AddKill(fractionId, amount);
			GangWarStats gangWarStats = GangWarCache.GetGangWarStats();
			NAPI.ClientEvent.TriggerClientEventForAll("UpdateGangWarStats", gangWarStats.Ballas, gangWarStats.Bloods, gangWarStats.Marabunta, gangWarStats.Families, gangWarStats.Vagos);
		}
	}
}
