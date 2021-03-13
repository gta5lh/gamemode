// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.Player
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Repositories.Models;
    using Gamemode.Repository;
    using GTANetworkAPI;

    public class CustomPlayer : Player
    {
        private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
        private Models.Admin.AdminRank adminRank;

        public CustomPlayer(NetHandle handle)
    : base(handle)
        {
        }

        public MuteState? MuteState { get; set; }

        public string ChatColor { get; set; }

        public string Username { get; set; }

        public long StaticId { get; set; }

        private InventoryWeapons InventoryWeapons;

        public bool Freezed { get; set; }

        public Models.Admin.AdminRank AdminRank
        {
            get => this.adminRank;

            set
            {
                this.adminRank = value;

                if (this.adminRank.IsAdmin())
                {
                    AdminsCache.LoadAdminToCache(this.StaticId, this.Name);
                    this.SetSharedData(DataKey.IsAdmin, true);
                }
                else
                {
                    AdminsCache.UnloadAdminFromCache(this.StaticId);
                    this.ResetSharedData(DataKey.IsAdmin);
                }
            }
        }

        public void CustomGiveWeapon(WeaponHash weaponHash, int amount)
        {
            this.GiveWeapon(weaponHash, amount);
            this.InventoryWeapons.AddWeapon(weaponHash);
        }

        public void CustomRemoveWeapon(WeaponHash weaponHash)
        {
            this.RemoveWeapon(weaponHash);
            this.InventoryWeapons.RemoveWeapon(weaponHash);
        }

        public void Unmute()
        {
            this.MuteState.Unmute();
            UserRepository.Unmute(this.StaticId);
            Logger.Info($"Player mute has expired. ID={this.StaticId}");
        }

        public static CustomPlayer LoadPlayerCache(CustomPlayer player, Repositories.Models.User user)
        {
            IdsCache.LoadIdsToCache(player.Id, user.Id);
            player.StaticId = user.Id;
            player.SetSharedData(DataKey.StaticId, player.StaticId);
            player.Name = user.Name;
            player.AdminRank = user.AdminRankId != null ? (Models.Admin.AdminRank)user.AdminRankId : 0;
            player.MuteState = new MuteState(user.MutedUntil, user.MutedById, user.MuteReason);

            player.InventoryWeapons = new InventoryWeapons();

            if (user.Weapons != null)
            {
                foreach (Repositories.Models.Weapon weapon in user.Weapons)
                {
                    player.CustomGiveWeapon(weapon.Hash, weapon.Amount);
                }
            }

            Logger.Info($"Loaded player to cache. ID={player.StaticId}");
            return player;
        }

        public static async Task UnloadPlayerCache(CustomPlayer player)
        {
            player.ResetData();
            player.ResetSharedData(DataKey.StaticId);
            player.AdminRank = 0;
            IdsCache.UnloadIdsFromCacheByDynamicId(player.Id);

            List<Weapon> weapons = new List<Weapon>();
            foreach (WeaponHash weaponHash in player.InventoryWeapons.GetAllWeapons())
            {
                weapons.Add(new Weapon(weaponHash, player.GetWeaponAmmo(weaponHash), player.StaticId));
            }

            await UserRepository.UpdateWeapons(player.StaticId, weapons);
            Logger.Info($"Unloaded player from cache. ID={player.StaticId}");
        }
    }
}
