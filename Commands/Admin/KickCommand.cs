using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class KickCommand : BaseCommandHandler
    {
        private const string KickCommandUsage = "Использование: /kick {player_id} {причина}. Пример: /ban 1 Бот";

        [Command("kick", KickCommandUsage, SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Ban(CustomPlayer admin, string targetIdInput = null, string reason = null)
        {
            if (targetIdInput == null || reason == null)
            {
                admin.SendChatMessage(KickCommandUsage);
                return;
            }

            ushort targetId;

            try
            {
                targetId = ushort.Parse(targetIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(KickCommandUsage);
                return;
            }

            CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
            if (targetPlayer == null || targetPlayer.AdminRank != 0)
            {
                admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
                return;
            }

            targetPlayer.Kick();
            Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} кикнул {targetPlayer.Name}. Причина: {reason}");
            this.Logger.Warn($"Administrator {admin.Name} kicked {targetPlayer.Name}");
        }
    }
}
