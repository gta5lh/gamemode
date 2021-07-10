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
	using GTANetworkAPI;
	using Rpc.User;

	public class CustomPlayer : Player
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private Models.Admin.AdminRank adminRank;
		private bool invisible;
		private bool spectating;
		private bool noclip;

		public CustomPlayer(NetHandle handle)
	: base(handle)
		{
		}

		public DateTime? LoggedInAt { get; set; }

		public MuteState? MuteState { get; set; }

		public string ChatColor { get; set; }

		public string Username { get; set; }

		public long StaticId { get; set; }

		private InventoryWeapons InventoryWeapons;

		public ushort? OneTimeVehicleId { get; set; }

		public long? fraction { get; set; }
		public long? FractionRank { get; set; }
		public string? FractionRankName { get; set; }

		public bool IsInWarZone { get; set; }

		public long CurrentExperience { get; set; }
		public long? RequiredExperience { get; set; }

		public long Money { get; set; }


		public bool Freezed { get; set; }

		public Vector3? SpectatePosition { get; set; }

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
					FractionsCache.LoadFractionMemberToCache((byte)this.fraction, this.StaticId, this.Name);
					this.SetBlipColor(GangUtil.BlipColorByGangId[this.fraction.Value]);
					return;
				}

				this.SetBlipColor(62);
			}
		}

		public bool Spectating
		{
			get => this.spectating;

			set
			{
				if (this.Noclip || this.Invisible)
				{
					return;
				}

				this.spectating = value;

				if (this.Spectating)
				{
					this.Transparency = 0;
					this.SetBlipColor(-1);
					this.RemoveAllWeapons();
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
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
					this.RemoveAllWeapons();
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
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
					this.SetBlipColor(-1);
					this.RemoveAllWeapons();
				}
				else
				{
					this.Transparency = 255;
					this.SetDefaultBlipColor();
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

			long fractionRank = (long)(this.FractionRank + 1);
			SetFractionResponse setFractionResponse;

			try
			{
				SetFractionRequest setFractionRequest = new SetFractionRequest();
				setFractionRequest.ID = this.StaticId;
				setFractionRequest.SetBy = this.StaticId;
				setFractionRequest.Fraction = this.Fraction.Value;
				setFractionRequest.Tier = fractionRank;

				setFractionResponse = await Infrastructure.RpcClients.UserService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			this.FractionRank = fractionRank;
			this.CurrentExperience = (short)(this.CurrentExperience - this.RequiredExperience.Value);
			this.RequiredExperience = setFractionResponse.TierRequiredExperience;
			this.FractionRankName = setFractionResponse.TierName;
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

				setFractionResponse = await Infrastructure.RpcClients.UserService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			this.FractionRank = fractionRank;
			this.CurrentExperience = (short)(setFractionResponse.TierRequiredExperience - 1);
			this.RequiredExperience = setFractionResponse.TierRequiredExperience;
			this.FractionRankName = setFractionResponse.TierName;
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

		public async void Unmute()
		{
			try
			{
				UnmuteRequest unmuteRequest = new UnmuteRequest();
				unmuteRequest.ID = this.StaticId;
				unmuteRequest.UnmutedBy = this.StaticId;

				await Infrastructure.RpcClients.UserService.UnmuteAsync(unmuteRequest);
			}
			catch (Exception)
			{
				return;
			}

			this.MuteState.Unmute();
			Logger.Info($"Player mute has expired. ID={this.StaticId}");
		}

		public static CustomPlayer LoadPlayerCache(CustomPlayer player, User user)
		{
			IdsCache.LoadIdsToCache(player.Id, user.ID);
			player.StaticId = user.ID;
			player.SetSharedData(DataKey.StaticId, player.StaticId);
			player.Name = user.Name;
			player.AdminRank = user.HasAdminRankID ? (Models.Admin.AdminRank)user.AdminRankID : 0;
			player.InventoryWeapons = new InventoryWeapons();
			player.CurrentExperience = user.Experience;
			player.Money = user.Money;
			player.LoggedInAt = DateTime.UtcNow;
			player.SetSkin(PedHash.Tramp01);

			DateTime? mutedUntil = user.MutedUntil != null ? user.MutedUntil.ToDateTime() : (DateTime?)null;
			player.MuteState = new MuteState(mutedUntil, user.MutedByID, user.MuteReason);

			if (user.HasFractionRankID)
			{
				player.Fraction = user.Fraction;
				player.FractionRank = user.FractionRank.Tier;
				player.FractionRankName = user.FractionRank.Name;
				player.RequiredExperience = user.FractionRank.RequiredExperience;
				player.SetSkin((PedHash)user.FractionRank.Skin);
			}

			if (user.Weapons != null)
			{
				foreach (Weapon weapon in user.Weapons)
				{
					player.CustomGiveWeapon((WeaponHash)weapon.Hash, weapon.Amount);
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

			List<Weapon> weapons = player.GetAllWeapons();

			try
			{
				SaveRequest saveRequest = new SaveRequest();
				saveRequest.ID = player.StaticId;
				saveRequest.Experience = player.CurrentExperience;
				saveRequest.Weapons.Add(weapons);
				saveRequest.Money = player.Money;

				await Infrastructure.RpcClients.UserService.SaveAsync(saveRequest);
			}
			catch (Exception)
			{
			}

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
			this.SetSharedData("blip_color", color);
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
	}
}
