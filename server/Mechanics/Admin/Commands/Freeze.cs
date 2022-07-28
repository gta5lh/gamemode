﻿using System;
using GTANetworkAPI;
using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Player.Models;

namespace Gamemode.Mechanics.Admin.Commands
{
	public class Freeze : BaseHandler
	{
		private const string FreezeUsage = "Использование: /freeze {player_id}. Пример: /freeze 10";

		[Command("freeze", FreezeUsage, Alias = "fe", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnFreeze(CPlayer admin, string? playerIdInput = null)
		{
			if (playerIdInput == null)
			{
				admin.SendChatMessage(FreezeUsage);
				return;
			}

			ushort playerId = 0;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(FreezeUsage);
				return;
			}

			CPlayer targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			targetPlayer.Freezed = !targetPlayer.Freezed;
			if (targetPlayer.Freezed && targetPlayer.IsInVehicle)
			{
				targetPlayer.WarpOutOfVehicle();
			}

			string freezeString = targetPlayer.Freezed ? "заморозил" : "разморозил";
			Cache.SendMessageToAllAdminsAction($"{admin.Name} {freezeString} {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} freezed({targetPlayer.Freezed}) {targetPlayer.Name}");
		}
	}
}
