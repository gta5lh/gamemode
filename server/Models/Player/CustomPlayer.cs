// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.Player
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Admin;
	using GamemodeCommon.Models.Data;
	using Gamemode.Services.Player;
	using GTANetworkAPI;
	using Rpc.Player;
	using Gamemode.Services;
	using System.Linq;
	using Gamemode.Cache.Player;
	using Gamemode.Models.Vip;

	public class CustomPlayer : GTANetworkAPI.Player
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private AdminRank adminRank;
		private VipRank vipRank;
		private bool invisible;
		private bool spectating;
		private bool noclip;
		private bool freezed;

		public CustomPlayer(NetHandle handle)
	: base(handle)
		{
		}

		public DateTime? LoggedInAt { get; set; }

		public MuteState? MuteState { get; set; }

		public string ChatColor { get; set; }

		public string PlayerName { get; set; }

		public long StaticId { get; set; }

		private InventoryWeapons InventoryWeapons;
		private List<Weapon> TemporaryWeapons;

		public ushort? OneTimeVehicleId { get; set; }

		public long? FractionRank { get; set; }

		public string? fractionRankName;
		public string? FractionRankName
		{
			get => this.fractionRankName;
			set
			{
				this.fractionRankName = value;
				this.TriggerEvent("FractionRankNameUpdated", this.fractionRankName);
			}
		}

		public bool IsInWarZone { get; set; }

		public long CurrentExperience { get; set; }
		public long? RequiredExperience { get; set; }

		public long money { get; set; }

		public long Money
		{
			get => this.money;

			set
			{
				this.money = value;
				this.TriggerEvent("MoneyUpdated", this.money);
			}
		}

		public bool Freezed
		{
			get => this.freezed;

			set
			{
				if (value == this.freezed) return;
				this.freezed = value;

				Gamemode.Services.Player.PlayerService.Freeze(this, this.freezed);
			}
		}

		public Vector3? SpectatePosition { get; set; }

		public long? fraction { get; set; }
		public long? Fraction
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

					this.TriggerEvent("FractionNameUpdated", GangUtil.GangReadableNameById[(byte)this.fraction]);
					FractionsCache.LoadFractionMemberToCache((byte)this.fraction, this.StaticId, this.Name);
					this.SetBlipColor(GangUtil.BlipColorByGangId[this.fraction.Value]);
					return;
				}

				this.SetBlipColor(62);
				this.TriggerEvent("FractionNameUpdated");
			}
		}

		public bool Spectating
		{
			get => this.spectating;

			set
			{
				this.spectating = value;

				if (this.Noclip)
				{
					return;
				}

				if (this.Invisible)
				{
					if (this.spectating) this.Freezed = true;
					else this.Freezed = false;

					return;
				}


				if (this.Spectating)
				{
					this.Transparency = 0;
					this.SetBlipColor(-1);
					this.SaveTemporaryWeapons();
					this.RemoveAllWeapons();
					this.Freezed = true;
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
					this.GiveAndResetTemporaryWeapons();
					this.Freezed = false;
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
					this.SetBlipColor(-1);
					this.SaveTemporaryWeapons();
					this.RemoveAllWeapons();
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
					this.GiveAndResetTemporaryWeapons();
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
					if (this.noclip) this.Freezed = true;
					else this.Freezed = false;

					return;
				}

				if (this.noclip)
				{
					this.Transparency = 0;
					this.SetBlipColor(-1);
					this.SaveTemporaryWeapons();
					this.RemoveAllWeapons();
					this.Freezed = true;
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
					this.GiveAndResetTemporaryWeapons();
					this.Freezed = false;
				}
			}
		}

		public AdminRank AdminRank
		{
			get => this.adminRank;

			set
			{
				this.adminRank = value;

				if (this.adminRank.IsAdmin())
				{
					AdminsCache.LoadAdminToCache(this.StaticId, this.Name);
					VipsCache.LoadVipToCache(this.StaticId, this.Name);
					this.SetSharedData(DataKey.IsAdmin, true);
				}
				else
				{
					AdminsCache.UnloadAdminFromCache(this.StaticId);
					VipsCache.UnloadVipFromCache(this.StaticId);
					this.ResetSharedData(DataKey.IsAdmin);
				}
			}
		}

		public VipRank VipRank
		{
			get => this.vipRank;

			set
			{
				this.vipRank = value;

				if (this.vipRank.IsVip())
				{
					VipsCache.LoadVipToCache(this.StaticId, this.Name);
				}
				else
				{
					VipsCache.UnloadVipFromCache(this.StaticId);
				}
			}
		}

		public async Task RankUp()
		{
			if (this.FractionRank >= 10)
			{
				return;
			}

			long fractionRank = (long)(this.FractionRank + 1);
			SetFractionResponse setFractionResponse;

			try
			{
				SetFractionRequest setFractionRequest = new SetFractionRequest();
				setFractionRequest.ID = this.StaticId;
				setFractionRequest.SetBy = this.StaticId;
				setFractionRequest.Fraction = this.Fraction.Value;
				setFractionRequest.Tier = fractionRank;

				setFractionResponse = await Infrastructure.RpcClients.PlayerService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			NAPI.Task.Run(() =>
			{
				this.FractionRank = fractionRank;
				this.CurrentExperience = (short)(this.CurrentExperience - this.RequiredExperience.Value);
				this.RequiredExperience = setFractionResponse.TierRequiredExperience;
				this.FractionRankName = setFractionResponse.TierName;
			});
		}

		public async Task RankDown()
		{
			if (this.FractionRank <= 1)
			{
				return;
			}

			byte fractionRank = (byte)(this.FractionRank - 1);
			SetFractionResponse setFractionResponse;

			try
			{
				SetFractionRequest setFractionRequest = new SetFractionRequest();
				setFractionRequest.ID = this.StaticId;
				setFractionRequest.SetBy = this.StaticId;
				setFractionRequest.Fraction = this.Fraction.Value;
				setFractionRequest.Tier = fractionRank;

				setFractionResponse = await Infrastructure.RpcClients.PlayerService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			NAPI.Task.Run(() =>
			{
				this.FractionRank = fractionRank;
				this.CurrentExperience = (short)(setFractionResponse.TierRequiredExperience - 1);
				this.RequiredExperience = setFractionResponse.TierRequiredExperience;
				this.FractionRankName = setFractionResponse.TierName;
			});
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return this.InventoryWeapons.HasWeapon(weaponHash);
		}

		public void CustomGiveWeapon(WeaponHash weaponHash, long amount)
		{
			this.GiveWeapon(weaponHash, (int)amount);
			this.InventoryWeapons.AddWeapon(weaponHash);
		}

		public void CustomRemoveWeapon(WeaponHash weaponHash)
		{
			this.RemoveWeapon(weaponHash);
			this.InventoryWeapons.RemoveWeapon(weaponHash);
		}

		public void CustomRemoveAllWeapons()
		{

			this.RemoveAllWeapons();
			this.InventoryWeapons = new InventoryWeapons();
		}

		public async Task Unmute()
		{
			try
			{
				UnmuteRequest unmuteRequest = new UnmuteRequest();
				unmuteRequest.ID = this.StaticId;
				unmuteRequest.UnmutedBy = this.StaticId;

				await Infrastructure.RpcClients.PlayerService.UnmuteAsync(unmuteRequest);
			}
			catch (Exception)
			{
				return;
			}

			this.MuteState.Unmute();
			Logger.Info($"Player mute has expired. ID={this.StaticId}");
		}

		public static CustomPlayer LoadPlayerCache(CustomPlayer player, Rpc.Player.Player playerToLoad)
		{
			IdsCache.LoadIdsToCache(player.Id, playerToLoad.ID);
			player.StaticId = playerToLoad.ID;
			player.SetSharedData(DataKey.StaticId, player.StaticId);
			player.Name = playerToLoad.Name;
			player.AdminRank = playerToLoad.HasAdminRankID ? (Models.Admin.AdminRank)playerToLoad.AdminRankID : 0;
			player.VipRank = playerToLoad.HasAdminRankID ? VipRank.Premium : 0;
			player.InventoryWeapons = new InventoryWeapons();
			player.CurrentExperience = playerToLoad.Experience;
			player.Money = playerToLoad.Money;
			player.LoggedInAt = DateTime.UtcNow;
			player.SetSkin(PedHash.Tramp01);

			DateTime? mutedUntil = playerToLoad.MutedUntil != null ? playerToLoad.MutedUntil.ToDateTime() : (DateTime?)null;
			player.MuteState = new MuteState(mutedUntil, playerToLoad.MutedByID, playerToLoad.MuteReason);

			if (playerToLoad.HasFractionRankID)
			{
				player.Fraction = playerToLoad.Fraction;
				player.FractionRank = playerToLoad.FractionRank.Tier;
				player.FractionRankName = playerToLoad.FractionRank.Name;
				player.RequiredExperience = playerToLoad.FractionRank.RequiredExperience;
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

		public static void UnloadPlayerCache(CustomPlayer player)
		{
			player.ResetData();
			player.ResetSharedData(DataKey.StaticId);
			player.AdminRank = 0;
			player.Fraction = null;
			IdsCache.UnloadIdsFromCacheByDynamicId(player.Id);
			Logger.Info($"Unloaded player from cache. ID={player.StaticId}");
		}

		public List<Weapon> GetAllWeapons()
		{
			List<Weapon> weapons = new List<Weapon>();
			foreach (WeaponHash weaponHash in this.InventoryWeapons.GetAllWeapons())
			{
				Weapon weapon = new Weapon();
				weapon.Hash = (long)weaponHash;
				weapon.Amount = this.GetWeaponAmmo(weaponHash);

				weapons.Add(weapon);
			}

			return weapons;
		}

		private void SetBlipColor(int color)
		{
			this.SetSharedData(DataKey.BlipColor, color);
		}

		private void SetDefaultBlipColor()
		{
			if (this.fraction != null)
			{
				this.SetBlipColor(GangUtil.BlipColorByGangId[this.fraction.Value]);
			}
			else
			{
				this.SetBlipColor(62);
			}
		}

		private void SaveTemporaryWeapons()
		{
			List<Weapon> weapons = this.GetAllWeapons();
			if (weapons == null || weapons.Count == 0) return;

			this.TemporaryWeapons = weapons;
		}

		private void GiveAndResetTemporaryWeapons()
		{
			if (this.TemporaryWeapons == null || this.TemporaryWeapons.Count == 0) return;

			List<Weapon> weapons = this.TemporaryWeapons.OrderByDescending(o => o.Amount).ToList();

			foreach (Weapon weapon in this.TemporaryWeapons)
			{
				this.CustomGiveWeapon((WeaponHash)weapon.Hash, 0);
				this.SetWeaponAmmo((WeaponHash)weapon.Hash, (int)weapon.Amount);
			}

			this.TemporaryWeapons.Clear();
		}

		public void SendNotification(string text, long delay, long closeTimeMs, string notificationType)
		{
			this.TriggerEvent("DisplayNotification", text, delay, closeTimeMs, notificationType);
		}
	}
}
