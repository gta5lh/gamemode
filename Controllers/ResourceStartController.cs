﻿// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using System;
    using Gamemode.Controllers;
    using Gamemode.Models.Npc;
    using Gamemode.Models.Player;
    using Gamemode.Models.Vehicle;
    using GTANetworkAPI;
    using Microsoft.Extensions.Caching.Memory;
    using NLog.Extensions.Logging;

    public class ResourceStartController : Script
    {
        private static IMemoryCache Cache;

        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.SetServerSettings();
            SpawnNpcs.CreateSpawnNpcs();

            RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CustomPlayer(handle);
            RAGE.Entities.Vehicles.CreateEntity = (NetHandle handle) => new CustomVehicle(handle);

            Cache = new MemoryCache(new MemoryCacheOptions { }, new NLogLoggerFactory());
            MoneyController.SetPaydayTimer();
        }

        private void SetServerSettings()
        {
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);
        }

        public static bool ShouldWait(ushort playerId)
        {
            string cacheKey = $"{CacheKeys.UserAuthenticationAction}{playerId}";

            if (Cache.Get<bool?>(cacheKey) != null)
            {
                return true;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2));
            Cache.Set(cacheKey, true, cacheEntryOptions);

            return false;
        }
    }

    public static class CacheKeys
    {
        public static string UserAuthenticationAction { get { return "UserAuthenticationAction"; } }
    }
}
