// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Player.Models
{
	using Gamemode.GameMechanics.Admin.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;

	public class CPlayer : GTANetworkAPI.Player
	{
		public CPlayer(NetHandle handle) : base(handle)
		{
		}

		public AdminRank AdminRank
		{
			get => this.adminRank;

			set
			{
				this.adminRank = value;

				if (this.adminRank.IsAdmin())
				{
					this.SetSharedData(DataKey.IsAdmin, true);
				}
				else
				{
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

		public string StaticId { get; set; }

		private AdminRank adminRank;

		private bool invisible;

		private bool spectating;

		private bool noclip;

		private bool freezed;

		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

		public static CPlayer LoadPlayerCache(CPlayer player, Rpc.Player.Player playerToLoad)
		{
			player.StaticId = playerToLoad.StaticID;
			player.AdminRank = playerToLoad.HasAdminRankID ? (AdminRank)playerToLoad.AdminRankID : 0;
			player.SetSharedData(DataKey.StaticId, player.StaticId);

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
	}
}
