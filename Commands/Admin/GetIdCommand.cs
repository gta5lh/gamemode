// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using System;
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class GetIdCommand : Script
    {
        private const string IdTypeStatic = "s";
        private const string IdTypeDynamic = "d";
        private const string GetIdCommandUsage = "Использование: /getid {s-статичный или d-динамичный} {id или имя}. Примеры: [/getid s 100], [/getid s lbyte00]";

        [Command("getid", GetIdCommandUsage, Alias = "gid", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public async Task GetId(Player player, string idType = null, string idOrUsername = null)
        {
            if (idType == null || idOrUsername == null || (idType != "s" && idType != "d"))
            {
                player.SendChatMessage(GetIdCommandUsage);
                return;
            }

            long? staticId = null;

            try
            {
                _ = long.Parse(idOrUsername);
            }
            catch (Exception)
            {
                try
                {
                    staticId = await ApiClient.ApiClient.IDByName(idOrUsername);
                }
                catch (Exception)
                {

                }
            }

            long? expectedId = null;

            if (idType == IdTypeStatic)
            {
                expectedId = staticId == null ? IdsCache.StaticIdByDynamic(idOrUsername) : staticId;
            }
            else
            {
                expectedId = staticId == null ? IdsCache.DynamicIdByStatic(idOrUsername) : IdsCache.DynamicIdByStatic(staticId.Value);
            }

            NAPI.Task.Run(() =>
            {
                if (expectedId == null)
                {
                    if (staticId == null)
                    {
                        player.SendChatMessage($"Пользователь с ID {idOrUsername} не найден");
                    }
                    else
                    {
                        player.SendChatMessage($"Пользователь с именем {idOrUsername} не найден");
                    }

                    return;
                }

                player.SendChatMessage($"ID  = {expectedId}");
            });
        }
    }
}
