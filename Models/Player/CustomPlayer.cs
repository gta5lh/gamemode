// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.Player
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Gangs;
    using Gamemode.Repositories.Models;
    using Gamemode.Repository;
    using GTANetworkAPI;

    public class CustomPlayer : Player
    {
        private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
        private Models.Admin.AdminRank adminRank;
        private bool invisible;
        private bool noclip;

        public CustomPlayer(NetHandle handle)
    : base(handle)
        {
        }

        public MuteState? MuteState { get; set; }

        public string ChatColor { get; set; }

        public string Username { get; set; }

        public long StaticId { get; set; }

        private InventoryWeapons InventoryWeapons;

        public byte? fraction { get; set; }
        public byte? FractionRank { get; set; }
        public string? FractionRankName { get; set; }

        public short CurrentExperience { get; set; }
        public short? RequiredExperience { get; set; }


        public bool Freezed { get; set; }

        public byte? Fraction
        {
            get => this.fraction;

            set
            {
                if (this.fraction != null)
                {
                    FractionsCache.UnloadFractionMemberFromCache((byte)this.fraction, this.StaticId);
                }

                this.fraction = value;

                if (this.fraction != null)
                {
                    FractionsCache.LoadFractionMemberToCache((byte)this.fraction, this.StaticId, this.Name);
                }
            }
        }

        public bool Invisible
        {
            get => this.invisible;

            set
            {
                if (this.Noclip)
                {
                    return;
                }

                this.invisible = value;

                if (this.Invisible)
                {
                    this.Transparency = 0;
                    this.RemoveAllWeapons();
                }
                else
                {
                    this.Transparency = 255;
                }
            }
        }

        public bool Noclip
        {
            get => this.noclip;

            set
            {
                this.noclip = value;

                if (this.Invisible)
                {
                    return;
                }

                if (this.noclip)
                {
                    this.Transparency = 0;
                    this.RemoveAllWeapons();
                }
                else
                {
                    this.Transparency = 255;
                }
            }
        }

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

        public async Task RankUp()
        {
            if (this.FractionRank >= 10)
            {
                return;
            }

            this.FractionRank++;
            this.CurrentExperience = 0;
            FractionRank fractionRank = await UserRepository.GetFractionRankByFractionAndTier((byte)this.Fraction, (byte)this.FractionRank);
            await UserRepository.SetFractionRank(this.StaticId, fractionRank.Id);
            this.RequiredExperience = fractionRank.RequiredExperienceToRankUp;
            this.FractionRankName = fractionRank.Name;
        }

        public async Task RankDown()
        {
            if (this.FractionRank <= 1)
            {
                return;
            }

            this.FractionRank--;
            FractionRank fractionRank = await UserRepository.GetFractionRankByFractionAndTier((byte)this.Fraction, (byte)this.FractionRank);
            await UserRepository.SetFractionRank(this.StaticId, fractionRank.Id);
            this.CurrentExperience = (short)(fractionRank.RequiredExperienceToRankUp - 1);
            this.RequiredExperience = fractionRank.RequiredExperienceToRankUp;
            this.FractionRankName = fractionRank.Name;
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
            player.Fraction = user.FractionId;
            player.FractionRank = user.FractionRank != null ? (byte?)user.FractionRank.Tier : null;
            player.FractionRankName = user.FractionRank != null ? user.FractionRank.Name : null; ;
            player.InventoryWeapons = new InventoryWeapons();
            player.CurrentExperience = user.CurrentExperience;
            player.RequiredExperience = user.FractionRank != null ? (short?)user.FractionRank.RequiredExperienceToRankUp : null;

            if (user.FractionRankId != null)
            {
                player.SetClothes(Clothes.GangClothes[(byte)user.FractionRankId]);
            }

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
            player.Fraction = null;
            IdsCache.UnloadIdsFromCacheByDynamicId(player.Id);

            List<Weapon> weapons = new List<Weapon>();
            foreach (WeaponHash weaponHash in player.InventoryWeapons.GetAllWeapons())
            {
                weapons.Add(new Weapon(weaponHash, player.GetWeaponAmmo(weaponHash), player.StaticId));
            }

            await UserRepository.SaveUser(player.StaticId, weapons, player.CurrentExperience);
            Logger.Info($"Unloaded player from cache. ID={player.StaticId}");
        }
    }
}
