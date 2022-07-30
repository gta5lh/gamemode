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

		private AdminRank adminRank;
		private bool invisible;
		private bool spectating;
		private bool noclip;
		private List<Rpc.Player.Weapon> TemporaryWeapons;

		private void SaveTemporaryWeapons()
		{
			List<Rpc.Player.Weapon> weapons = this.GetAllWeapons();
			if (weapons == null || weapons.Count == 0) return;

			this.TemporaryWeapons = weapons;
		}

		private void GiveAndResetTemporaryWeapons()
		{
			if (this.TemporaryWeapons == null || this.TemporaryWeapons.Count == 0) return;

			List<Rpc.Player.Weapon> weapons = this.TemporaryWeapons.OrderByDescending(o => o.Amount).ToList();

			foreach (Rpc.Player.Weapon weapon in this.TemporaryWeapons)
			{
				this.CustomGiveWeapon((WeaponHash)weapon.Hash, 0);
				this.SetWeaponAmmo((WeaponHash)weapon.Hash, (int)weapon.Amount);
			}

			this.TemporaryWeapons.Clear();
		}
	}
}
