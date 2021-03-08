// <copyright file="Mute.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using System;
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using Gamemode.Models.User;
    using Gamemode.Repository;
    using Gamemode.Utils;
    using GTANetworkAPI;

    public class MuteCommand : BaseCommandHandler
    {
        private const string MuteCommandUsage = "Использование: /mute {static_id} {минуты} {причина}. Пример: /mute 1 100 Оскорбления";
        private const string UnmuteCommandUsage = "Использование: /mute {static_id}.~n~ Пример: /unmute 1";
        private const int monthInMinutes = 44640;

        [AdminMiddleware(AdminRank.Junior)]
        [Command("mute", MuteCommandUsage, Alias = "m", GreedyArg = true, Hide = true)]
        public async Task Mute(CustomPlayer admin, string playerId = null, string durationMinutes = null, string reason = null)
        {
            if (playerId == null || durationMinutes == null || reason == null)
            {
                admin.SendChatMessage(MuteCommandUsage);
                return;
            }

            long targetId;
            int duration;

            try
            {
                targetId = long.Parse(playerId);
                duration = int.Parse(durationMinutes);
            }
            catch (Exception)
            {
                admin.SendChatMessage(MuteCommandUsage);
                return;
            }

            if (duration == 0 || duration > monthInMinutes)
            {
                admin.SendChatMessage($"Мут можно выдать минимум на 1 минуту, максимум на {monthInMinutes}");
                return;
            }

            User target = await UserRepository.Mute(targetId, duration, reason, admin.StaticId);
            if (target == null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
                if (targetPlayer != null)
                {
                    targetPlayer.MuteState = target.MuteState;
                }

                Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал мут {target.Username} на {duration} минут. Причина: {reason}");
                this.Logger.Warn($"Administrator {admin.Name} muted {target.Username} for {duration} minutes");
            });
        }

        [Command("unmute", UnmuteCommandUsage, Alias = "um", GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async Task Unmute(CustomPlayer admin, string playerId = null)
        {
            if (playerId == null)
            {
                admin.SendChatMessage(UnmuteCommandUsage);
                return;
            }

            long targetId;

            try
            {
                targetId = long.Parse(playerId);
            }
            catch (Exception)
            {
                admin.SendChatMessage(UnmuteCommandUsage);
                return;
            }

            User target = await UserRepository.Unmute(targetId);
            if (target == null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
                if (targetPlayer != null)
                {
                    targetPlayer.MuteState = new MuteState();
                }

                Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял мут {target.Username}");
                this.Logger.Warn($"Administrator {admin.Name} unmuted {target.Username}");
            });
        }
    }
}
