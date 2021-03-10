﻿using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class GodmodCommand : BaseCommandHandler
    {
        private const string GodmodCommandUsage = "Использование: /godmod. Пример: /gm";

        [AdminMiddleware(AdminRank.Junior)]
        [Command("godmod", GodmodCommandUsage, Alias = "gm", GreedyArg = true, Hide = true)]
        public void Godmod(CustomPlayer admin)
        {
            NAPI.ClientEvent.TriggerClientEvent(admin, "SetGodmod");
        }
    }
}
