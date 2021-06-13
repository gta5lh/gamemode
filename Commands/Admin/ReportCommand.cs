<<<<<<< Updated upstream
﻿using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class ReportCommand : BaseCommandHandler
    {
        private const string ReportAnswerCommandUsage = "Использование: /reportanswer. Пример: /ra";

        [Command("reportanswer", ReportAnswerCommandUsage, Alias = "ra", GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void ReportAnswer(CustomPlayer admin, string targetIdInput = null, string message = null)
        {
            if (targetIdInput == null || message == null)
            {
                admin.SendChatMessage(ReportAnswerCommandUsage);
                return;
            }

            ushort playerId = 0;

            try
            {
                playerId = ushort.Parse(targetIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(ReportAnswerCommandUsage);
                return;
            }

            CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
            if (targetPlayer == null)
            {
                admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
                return;
            }

            targetPlayer.SendChatMessage($"{ChatColor.AdminReportAnswerColor}Администратор {admin.Name} ответил вам: {message}");
            AdminsCache.SendMessageToAllAdminsReportAnswer($"{admin.Name} ответил {targetPlayer.Name} [{targetPlayer.Id}]: {message}");
        }
    }
}
=======
﻿using System;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class ReportCommand : BaseCommandHandler
    {
        private const string ReportAnswerCommandUsage = "Использование: /reportanswer. Пример: /ra";

        [Command("reportanswer", ReportAnswerCommandUsage, Alias = "ra", GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async void ReportAnswer(CustomPlayer admin, string targetIdInput = null, string message = null)
        {
            if (targetIdInput == null || message == null)
            {
                admin.SendChatMessage(ReportAnswerCommandUsage);
                return;
            }

            ushort playerId = 0;

            try
            {
                playerId = ushort.Parse(targetIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(ReportAnswerCommandUsage);
                return;
            }

            ReportAnswer reportAnswer = new ReportAnswer(1, admin.StaticId, message);

            try
            {
                await ApiClient.ApiClient.AnswerReport(reportAnswer);
            }
            catch (Exception)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Что-то пошло не так, попробуйте еще раз."));
                return;
            }

            CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
            if (targetPlayer == null)
            {
                admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
                return;
            }

            targetPlayer.SendChatMessage($"{ChatColor.AdminReportAnswerColor}Администратор {admin.Name} ответил вам: {message}");
            AdminsCache.SendMessageToAllAdminsReportAnswer($"{admin.Name} ответил {targetPlayer.Name} [{targetPlayer.Id}]: {message}");
        }
    }
}
>>>>>>> Stashed changes
