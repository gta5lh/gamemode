// <copyright file="AdminChatCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class AdminChatCommand : Script
    {
        private const string AdminChatCommandUsage = "Использование: /adminchat {сообщение}. Пример: /ac Привет коллеги!";

        [Command("adminchat", AdminChatCommandUsage, Alias = "ac", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void AdminChat(CustomPlayer admin, string message = null)
        {
            if (message == null)
            {
                admin.SendChatMessage(AdminChatCommandUsage);
                return;
            }

            AdminsCache.SendMessageToAllAdminsChat($"{admin.Name} [{admin.StaticId}]: {message}");
        }
    }
}
