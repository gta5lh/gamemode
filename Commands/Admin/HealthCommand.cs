using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class HealthCommand : BaseCommandHandler
    {
        private const string HealthCommandUsage = "Использование: /health {h-здоровье или a-броня} {player_id}. Пример: /health h 10";
        private const int MaxHealth = 100;

        [Command("health", HealthCommandUsage, Alias = "h", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Freeze(CustomPlayer admin, string healthType = null, string playerIdInput = null)
        {
            if (healthType == null || playerIdInput == null || (healthType != "h" && healthType != "a"))
            {
                admin.SendChatMessage(HealthCommandUsage);
                return;
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
                targetPlayer.Health = MaxHealth;
            }
            else
            {
                targetPlayer.Armor = MaxHealth;
            }

            string healthTypeString = healthType == "h" ? "здоровье" : "броню";
            AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} [{admin.StaticId}] восстановил {healthTypeString} {targetPlayer.Name} [{targetPlayer.StaticId}]");
            this.Logger.Warn($"Administrator {admin.Name} healed({healthType}) {targetPlayer.Name}");
        }
    }
}
