﻿// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using System;
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using Gamemode.Repository;
    using Gamemode.Utils;
    using GTANetworkAPI;

    public class SetAdminRankCommand : BaseCommandHandler
    {
        private const string SetAdminRankCommandUsage = "Использование: /setadminrank {static_id} {rank_id}. Пример: /setadminrank 0 1";

        [Command("setadminrank", SetAdminRankCommandUsage, Alias = "sar", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Owner)]
        public async Task SetAdminRank(CustomPlayer admin, string inputTargetStaticId = null, string rankId = null)
        {
            if (inputTargetStaticId == null || rankId == null)
            {
                admin.SendChatMessage(SetAdminRankCommandUsage);
                return;
            }

            long targetStaticId;
            AdminRank adminRank;

            try
            {
                targetStaticId = long.Parse(inputTargetStaticId);
                adminRank = rankId.ToAdminRank();
            }
            catch (Exception)
            {
                admin.SendChatMessage("static_id должен быть цифрой. rank_id должен быть от 0 до 5 включительно");
                return;
            }

            Repositories.Models.User targetUser = await UserRepository.GetAdminRankById(targetStaticId);
            if (targetUser == null)
            {
                NAPI.Task.Run(() =>
                {
                    admin.SendChatMessage($"Пользователь с static_id {targetStaticId} отсутствует");
                });

                return;
            }

            AdminRank adminRankBeforeUpdate = targetUser.AdminRankId != null ? (Models.Admin.AdminRank)targetUser.AdminRankId : 0;

            targetUser = await UserRepository.SetAdminRank(targetStaticId, adminRank);
            NAPI.Task.Run(() =>
            {
                if (targetUser == null)
                {
                    admin.SendChatMessage($"Пользователь с static_id {targetStaticId} отсутствует");
                    return;
                }

                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);
                if (targetPlayer != null)
                {
                    targetPlayer.AdminRank = adminRank;
                }

                bool isNewRankAdmin = adminRank.IsAdmin();

                if ((adminRankBeforeUpdate >= adminRank && adminRank != 0) || (adminRankBeforeUpdate == 0 && adminRank == 0))
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} [{admin.StaticId}] сменил должность администратора {targetUser.Name} [{targetUser.Id}] на {adminRank.ToPosition()}");
                    this.Logger.Warn($"Administrator {admin.Name} set {targetUser.Name} as administrator [{adminRank}]");
                    return;
                }

                if (isNewRankAdmin)
                {
                    Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} назначил {targetUser.Name} на должность {adminRank.ToPosition()} администратор. Наши поздравления!");
                    this.Logger.Warn($"Administrator {admin.Name} set {targetUser.Name} as administrator [{adminRank}]");
                }
                else
                {
                    Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял {targetUser.Name} с должности администратора");
                    this.Logger.Warn($"Administrator {admin.Name} removed {targetUser.Name} from administrators");
                }
            });
        }
    }
}
