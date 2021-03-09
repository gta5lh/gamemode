// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class EspCommand : Script
    {
        private const string EspCommandUsage = "Использование: /esp]";

        [Command("esp", EspCommandUsage, Alias = "e", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async Task Esp(CustomPlayer admin)
        {
            admin.EspEnabled = !admin.EspEnabled;
            NAPI.ClientEvent.TriggerClientEvent(admin, "SetAdminEspState", admin.EspEnabled);
            admin.SendChatMessage("Ваш ESP " + (admin.EspEnabled ? "включен" : "выключен"));
        }
    }
}
