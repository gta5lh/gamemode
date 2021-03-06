// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System;
    using GTANetworkAPI;

    public class PlayerCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerCache"/> class.
        /// </summary>
        public PlayerCache()
        {
            this.MuteState = new MuteState();
        }

        public MuteState MuteState { get; }

        public string ChatColor { get; }

        public string Language { get; set; }

        
        //public void SetLanguage(string language)
        //{
        //    this.Language = language
        //}

        public static PlayerCache GetPlayerCache(Player player)
        {
            if (player.HasData(DataKey.PlayerCache))
            {
                return player.GetData<PlayerCache>(DataKey.PlayerCache);
            }

            return LoadPlayerCache(player);
        }


        private static PlayerCache LoadPlayerCache(Player player)
        {
            PlayerCache playerCache = new PlayerCache();
            player.SetData(DataKey.PlayerCache, new PlayerCache());
            return playerCache;
        }
    }
}
