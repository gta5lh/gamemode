// <copyright file="CPlayer.Admin.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System.Collections.Generic;
	using System.Linq;
	using Gamemode.Game.Admin.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;

	public partial class CPlayer
	{
		public Vector3? SpectatePosition { get; set; }

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
					this.Freezed = this.spectating;

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
					this.Freezed = this.noclip;

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

		private AdminRank adminRank;
		private bool invisible;
		private bool spectating;
		private bool noclip;
		private List<Rpc.Player.Weapon> temporaryWeapons;

		private void SaveTemporaryWeapons()
		{
			List<Rpc.Player.Weapon> weapons = this.GetAllWeapons();
			if (weapons == null || weapons.Count == 0)
			{
				return;
			}

			this.temporaryWeapons = weapons;
		}

		private void GiveAndResetTemporaryWeapons()
		{
			if (this.temporaryWeapons == null || this.temporaryWeapons.Count == 0)
			{
				return;
			}

			List<Rpc.Player.Weapon> weapons = this.temporaryWeapons.OrderByDescending(o => o.Amount).ToList();

			foreach (Rpc.Player.Weapon weapon in this.temporaryWeapons)
			{
				this.CustomGiveWeapon((WeaponHash)weapon.Hash, 0);
				this.SetWeaponAmmo((WeaponHash)weapon.Hash, (int)weapon.Amount);
			}

			this.temporaryWeapons.Clear();
		}
	}
}
