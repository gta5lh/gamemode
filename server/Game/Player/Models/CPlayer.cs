// <copyright file="CPlayer.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Gamemode.Game.Admin.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;
	using Rpc.Player;

	public partial class CPlayer : GTANetworkAPI.Player
	{
		public CPlayer(NetHandle handle)
			: base(handle)
		{
		}

		public ushort? OneTimeVehicleId { get; set; }

		public MuteState? MuteState { get; set; }

		public VipRank VipRank
		{
			get => this.vipRank;

			set
			{
				this.vipRank = value;

				if (this.vipRank.IsVip())
				{
					Vip.Cache.LoadVipToCache(this.StaticId, this.Name);
				}
				else
				{
					Vip.Cache.UnloadVipFromCache(this.StaticId);
				}
			}
		}

		public bool Freezed
		{
			get => this.freezed;

			set
			{
				if (value == this.freezed)
				{
					return;
				}

				this.freezed = value;

				// TODO
				// Gamemode.Services.Player.PlayerService.Freeze(this, this.freezed);
			}
		}

		// Primary Key in database.
		public Guid PKId { get; set; }

		public string StaticId { get; set; }

		private VipRank vipRank;
		private bool freezed;
		private InventoryWeapons inventoryWeapons;

		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

		public static CPlayer LoadPlayerCache(CPlayer player, Rpc.Player.Player playerToLoad)
		{
			IdsCache.LoadIdsToCache(player.Id, playerToLoad.StaticID);
			player.PKId = Guid.Parse(playerToLoad.ID);
			player.StaticId = playerToLoad.StaticID;
			player.SetSharedData(DataKey.StaticId, player.StaticId);
			player.Name = playerToLoad.Name;
			player.AdminRank = playerToLoad.HasAdminRankID ? (AdminRank)playerToLoad.AdminRankID : 0;
			player.VipRank = playerToLoad.HasAdminRankID ? VipRank.Premium : 0;
			player.inventoryWeapons = new InventoryWeapons();

			// player.CurrentExperience = playerToLoad.Experience;
			// player.Money = playerToLoad.Money;
			// player.LoggedInAt = DateTime.UtcNow;
			player.SetSkin(PedHash.Tramp01);

			DateTime? mutedUntil = playerToLoad.MutedUntil != null ? playerToLoad.MutedUntil.ToDateTime() : (DateTime?)null;
			player.MuteState = new MuteState(mutedUntil, playerToLoad.MutedByID, playerToLoad.MuteReason);

			if (playerToLoad.HasFractionRankID)
			{
				// player.Fraction = playerToLoad.Fraction;
				// player.FractionRank = playerToLoad.FractionRank.Tier;
				// player.FractionRankName = playerToLoad.FractionRank.Name;
				// player.RequiredExperience = playerToLoad.FractionRank.RequiredExperience;
				player.SetSkin((PedHash)playerToLoad.FractionRank.Skin);
			}

			if (playerToLoad.Weapons != null)
			{
				List<Weapon> weapons = playerToLoad.Weapons.OrderByDescending(o => o.Amount).ToList();

				foreach (Weapon weapon in playerToLoad.Weapons)
				{
					player.CustomGiveWeapon((WeaponHash)weapon.Hash, 0);
					player.SetWeaponAmmo((WeaponHash)weapon.Hash, (int)weapon.Amount);
				}
			}

			Logger.Info($"Loaded player to cache. ID={player.StaticId}");
			return player;
		}

		public static void UnloadPlayerCache(CPlayer player)
		{
			player.ResetData();
			player.ResetSharedData(DataKey.StaticId);
			player.AdminRank = 0;

			// player.Fraction = null;
			IdsCache.UnloadIdsFromCacheByDynamicId(player.Id);
			Logger.Info($"Unloaded player from cache. ID={player.StaticId}");
		}

		private void SetBlipColor(int color)
		{
			this.SetSharedData(DataKey.BlipColor, color);
		}

		private void SetDefaultBlipColor()
		{
			// TODO
			// if (this.fraction != null)
			// {
			//  this.SetBlipColor(GangUtil.BlipColorByGangId[this.fraction.Value]);
			// }
			// else
			// {
			this.SetBlipColor(62);

			// }
		}

		public void SendNotification(string text, long delay, long closeTimeMs, string notificationType)
		{
			this.TriggerEvent("DisplayNotification", text, delay, closeTimeMs, notificationType);
		}

		public async Task Unmute()
		{
			try
			{
				UnmuteRequest unmuteRequest = new UnmuteRequest(this.StaticId, this.PKId);
				await Infrastructure.RpcClients.PlayerService.UnmuteAsync(unmuteRequest);
			}
			catch (Exception)
			{
				return;
			}

			this.MuteState?.Unmute();
			Logger.Info($"Player mute has expired. ID={this.StaticId}");
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return this.inventoryWeapons.HasWeapon(weaponHash);
		}

		public void CustomGiveWeapon(WeaponHash weaponHash, long amount)
		{
			this.GiveWeapon(weaponHash, (int)amount);
			this.inventoryWeapons.AddWeapon(weaponHash);
		}

		public void CustomRemoveWeapon(WeaponHash weaponHash)
		{
			this.RemoveWeapon(weaponHash);
			this.inventoryWeapons.RemoveWeapon(weaponHash);
		}

		public void CustomRemoveAllWeapons()
		{
			this.RemoveAllWeapons();
			this.inventoryWeapons = new InventoryWeapons();
		}

		public List<Weapon> GetAllWeapons()
		{
			List<Weapon> weapons = new List<Weapon>();
			foreach (WeaponHash weaponHash in this.inventoryWeapons.GetAllWeapons())
			{
				Weapon weapon = new Weapon();
				weapon.Hash = (long)weaponHash;
				weapon.Amount = this.GetWeaponAmmo(weaponHash);

				weapons.Add(weapon);
			}

			return weapons;
		}
	}
}
