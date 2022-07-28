// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Player.Models
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Gamemode.Mechanics.Admin.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;
	using Rpc.Player;

	public class CPlayer : GTANetworkAPI.Player
	{
		public CPlayer(NetHandle handle) : base(handle)
		{
		}

		public ushort? OneTimeVehicleId { get; set; }

		public Vector3? SpectatePosition { get; set; }

		public MuteState? MuteState { get; set; }

		public AdminRank AdminRank
		{
			get => this.adminRank;

			set
			{
				this.adminRank = value;

				if (this.adminRank.IsAdmin())
				{
					Admin.Cache.LoadAdminToCache(this.StaticId, this.Name);
					this.SetSharedData(DataKey.IsAdmin, true);
				}
				else
				{
					Admin.Cache.UnloadAdminFromCache(this.StaticId);
					this.ResetSharedData(DataKey.IsAdmin);
				}
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

		public bool Freezed
		{
			get => this.freezed;

			set
			{
				if (value == this.freezed) return;
				this.freezed = value;

				// TODO
				// Gamemode.Services.Player.PlayerService.Freeze(this, this.freezed);
			}
		}

		// Primary Key in database.
		public Guid PKId { get; set; }

		public string StaticId { get; set; }

		private AdminRank adminRank;

		private bool invisible;

		private bool spectating;

		private bool noclip;

		private bool freezed;

		private InventoryWeapons InventoryWeapons;

		private List<Weapon> TemporaryWeapons;
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

		public static CPlayer LoadPlayerCache(CPlayer player, Rpc.Player.Player playerToLoad)
		{
			IdsCache.LoadIdsToCache(player.Id, playerToLoad.StaticID);
			player.PKId = Guid.Parse(playerToLoad.ID);
			player.StaticId = playerToLoad.StaticID;
			player.AdminRank = playerToLoad.HasAdminRankID ? (AdminRank)playerToLoad.AdminRankID : 0;
			player.SetSharedData(DataKey.StaticId, player.StaticId);

			DateTime? mutedUntil = playerToLoad.MutedUntil != null ? playerToLoad.MutedUntil.ToDateTime() : (DateTime?)null;
			player.MuteState = new MuteState(mutedUntil, playerToLoad.MutedByID, playerToLoad.MuteReason);

			Logger.Info($"Loaded player to cache. ID={player.StaticId}");
			return player;
		}

		public static void UnloadPlayerCache(CPlayer player)
		{
			player.ResetData();
			player.ResetSharedData(DataKey.StaticId);
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
			// 	this.SetBlipColor(GangUtil.BlipColorByGangId[this.fraction.Value]);
			// }
			// else
			// {
			this.SetBlipColor(62);
			// }
		}

		private void SaveTemporaryWeapons()
		{
			// TODO
			// List<Weapon> weapons = this.GetAllWeapons();
			// if (weapons == null || weapons.Count == 0) return;

			// this.TemporaryWeapons = weapons;
		}

		private void GiveAndResetTemporaryWeapons()
		{
			// TODO
			// if (this.TemporaryWeapons == null || this.TemporaryWeapons.Count == 0) return;

			// List<Weapon> weapons = this.TemporaryWeapons.OrderByDescending(o => o.Amount).ToList();

			// foreach (Weapon weapon in this.TemporaryWeapons)
			// {
			// 	this.CustomGiveWeapon((WeaponHash)weapon.Hash, 0);
			// 	this.SetWeaponAmmo((WeaponHash)weapon.Hash, (int)weapon.Amount);
			// }

			// this.TemporaryWeapons.Clear();
		}

		public void SendNotification(string text, long delay, long closeTimeMs, string notificationType)
		{
			this.TriggerEvent("DisplayNotification", text, delay, closeTimeMs, notificationType);
		}

		public async Task Unmute()
		{
			try
			{
				UnmuteRequest unmuteRequest = new UnmuteRequest(this.StaticId, this.StaticId);
				await Infrastructure.RpcClients.PlayerService.UnmuteAsync(unmuteRequest);
			}
			catch (Exception)
			{
				return;
			}

			if (this.MuteState != null)
			{
				this.MuteState.Unmute();
			}
			Logger.Info($"Player mute has expired. ID={this.StaticId}");
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
	}
}
