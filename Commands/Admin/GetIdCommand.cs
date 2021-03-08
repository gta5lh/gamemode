// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class GetIdCommand : Script
    {
        private const string IdTypeStatic = "s";
        private const string IdTypeDynamic = "d";
        private const string GetIdCommandUsage = "Использование: /getid {s-статичный или d-динамичный} {id}. Пример: /getid s 100";

        [Command("getid", GetIdCommandUsage, Alias = "gid", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void GetId(Player player, string idType = null, string id = null)
        {
            if (idType == null || id == null || (idType != "s" && idType != "d"))
            {
                player.SendChatMessage(GetIdCommandUsage);
                return;
            }

            long? expectedId = null;

            switch (idType)
            {
                case IdTypeStatic:
                    expectedId = IdsCache.StaticIdByDynamic(id);

                    break;

                case IdTypeDynamic:
                    expectedId = IdsCache.DynamicIdByStatic(id);

                    break;
            }

            if (expectedId == null)
            {
                player.SendChatMessage($"Пользователь с ID {id} не найден");
                return;
            }

            player.SendChatMessage($"ID  = {expectedId}");
        }
    }
}
