// <copyright file="Freeze.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

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

			ushort playerId;
			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(FreezeUsage);
				return;
			}

			CPlayer targetPlayer = Gamemode.Game.Player.Util.GetById(playerId);
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
