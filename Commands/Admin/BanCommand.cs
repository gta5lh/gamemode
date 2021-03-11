using System;
using System.Threading.Tasks;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using Gamemode.Models.User;
using Gamemode.Repository;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class BanCommand : BaseCommandHandler
    {
        private const string BanCommandUsage = "Использование: /ban {static_id} {дни} {причина}. Пример: /ban 1 31 Спидхак";
        private const string UnbanCommandUsage = "Использование: /unban {static_id}";
        private const int BanInDays = 99999;

        [Command("ban", BanCommandUsage, SensitiveInfo = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async Task Ban(CustomPlayer admin, string staticIdInput = null, string durationDays = null, string reason = null)
        {
            if (staticIdInput == null || durationDays == null || reason == null)
            {
                admin.SendChatMessage(BanCommandUsage);
                return;
            }

            long staticId;
            int duration;

            try
            {
                staticId = long.Parse(staticIdInput);
                duration = int.Parse(durationDays);
            }
            catch (Exception)
            {
                admin.SendChatMessage(BanCommandUsage);
                return;
            }

            if (duration <= 0 || duration > BanInDays)
            {
                admin.SendChatMessage($"Бан можно выдать минимум на 1 день, максимум на {BanInDays}");
                return;
            }

            User user = await UserRepository.Ban(staticId, duration, reason, admin.StaticId);
            if (user == null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал бан {user.Username} на {duration} дней. Причина: {reason}");
                this.Logger.Warn($"Administrator {admin.Name} banned {user.Username} for {duration} days");

                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
                if (targetPlayer != null)
                {
                    targetPlayer.Ban();
                }
            });
        }

        [Command("unban", UnbanCommandUsage, SensitiveInfo = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async Task Unban(CustomPlayer admin, string staticIdInput = null)
        {
            if (staticIdInput == null)
            {
                admin.SendChatMessage(UnbanCommandUsage);
                return;
            }

            long staticId;

            try
            {
                staticId = long.Parse(staticIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(UnbanCommandUsage);
                return;
            }

            User user = await UserRepository.Unban(staticId);
            if (user == null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял бан {user.Username}");
                this.Logger.Warn($"Administrator {admin.Name} unbanned {user.Username}");
            });
        }
    }
}
