﻿// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.Player
{
    using Gamemode.Models.Admin;
    using Gamemode.Models.User;
    using Gamemode.Repository;
    using GTANetworkAPI;

    public class CustomPlayer : Player
    {
        private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

        public CustomPlayer(NetHandle handle)
    : base(handle)
        {
        }

        public MuteState? MuteState { get; set; }

        public string ChatColor { get; set; }

        public string Username { get; set; }

        public long StaticId { get; set; }

        public AdminRank AdminRank { get; set; }

        public void Unmute()
        {
            this.MuteState.Unmute();
            UserRepository.Unmute(this.StaticId);
            Logger.Debug($"Player mute has expired. ID={this.StaticId}");
        }

        public static CustomPlayer LoadPlayerCache(CustomPlayer player, User user)
        {
            IdsCache.LoadIdsToCache(player.Id, user.Id);
            player.StaticId = user.Id;
            player.Name = user.Username;
            player.AdminRank = user.AdminRank;
            player.MuteState = (user.MuteState == null) ? new MuteState() : user.MuteState;

            Logger.Debug($"Loaded player to cache. ID={player.StaticId}");
            return player;
        }

        public static void UnloadPlayerCache(CustomPlayer player)
        {
            player.ResetData();
            IdsCache.UnloadIdsFromCacheByDynamicId(player.Id);
            Logger.Debug($"Unloaded player from cache. ID={player.StaticId}");
        }
    }
}
