// <copyright file="Health.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Health : BaseHandler
	{
		private const string HealthUsage = "Использование: /health {h-здоровье или a-броня} {player_id}. Пример: /health h 10";
		private const int MaxAmount = 100;

		[Command("health", HealthUsage, Alias = "h", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnHealth(CPlayer admin, string? healthType = null, string? playerIdInput = null, string? amountInput = null)
		{
			if (healthType == null || playerIdInput == null || (healthType != "h" && healthType != "a"))
			{
				admin.SendChatMessage(HealthUsage);
				return;
			}

			int amount = MaxAmount;
			if (amountInput != null)
			{
				try
				{
					amount = int.Parse(amountInput);
				}
				catch
				{
					admin.SendChatMessage("Количество должно быть числом, либо пустым значением!");
					return;
				}
			}

			ushort playerId;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(HealthUsage);
				return;
			}

			CPlayer targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			if (healthType == "h")
			{
				targetPlayer.Health = amount;
			}
			else
			{
				targetPlayer.Armor = amount;
			}

			string healthTypeString = healthType == "h" ? "здоровье" : "броню";
			Cache.SendMessageToAllAdminsAction($"{admin.Name} восстановил {healthTypeString} {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} healed({healthType}) {targetPlayer.Name}");
		}
	}
}
