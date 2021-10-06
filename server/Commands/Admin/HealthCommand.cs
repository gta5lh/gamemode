using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Gamemode.Cache.Player;

namespace Gamemode.Commands.Admin
{
	public class HealthCommand : BaseCommandHandler
	{
		private const string HealthCommandUsage = "Использование: /health {h-здоровье или a-броня} {player_id}. Пример: /health h 10";
		private const int MaxAmount = 100;

		[Command("health", HealthCommandUsage, Alias = "h", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Health(CustomPlayer admin, string healthType = null, string playerIdInput = null, string amountInput = null)
		{
			if (healthType == null || playerIdInput == null || (healthType != "h" && healthType != "a"))
			{
				admin.SendChatMessage(HealthCommandUsage);
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
				admin.SendChatMessage(HealthCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
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
			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} восстановил {healthTypeString} {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} healed({healthType}) {targetPlayer.Name}");
		}
	}
}
