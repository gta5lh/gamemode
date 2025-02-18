﻿// <copyright file="Weapon.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Weapon
{
	using System.Collections.Generic;
	using System.Collections.Immutable;
	using GTANetworkAPI;

	public static class Weapon
	{
		public static readonly ImmutableDictionary<WeaponHash, string> AmmoTypeByWeapon = new Dictionary<WeaponHash, string>()
		{
			{ WeaponHash.Pistol, "1" },
			{ WeaponHash.Revolver, "1" },
			{ WeaponHash.Revolver_mk2, "1" },
			{ WeaponHash.Pistol_mk2, "1" },
			{ WeaponHash.Combatpistol, "1" },
			{ WeaponHash.Appistol, "1" },
			{ WeaponHash.Pistol50, "1" },
			{ WeaponHash.Snspistol, "1" },
			{ WeaponHash.Snspistol_mk2, "1" },
			{ WeaponHash.Heavypistol, "1" },
			{ WeaponHash.Flaregun, "1" },
			{ WeaponHash.Microsmg, "2" },
			{ WeaponHash.Smg, "2" },
			{ WeaponHash.Smg_mk2, "2" },
			{ WeaponHash.Assaultsmg, "2" },
			{ WeaponHash.Combatpdw, "2" },
			{ WeaponHash.Machinepistol, "2" },
			{ WeaponHash.Minismg, "2" },
			{ WeaponHash.Assaultrifle, "3" },
			{ WeaponHash.Assaultrifle_mk2, "3" },
			{ WeaponHash.Carbinerifle, "3" },
			{ WeaponHash.Carbinerifle_mk2, "3" },
			{ WeaponHash.Advancedrifle, "3" },
			{ WeaponHash.Specialcarbine, "3" },
			{ WeaponHash.Bullpuprifle, "3" },
			{ WeaponHash.Bullpuprifle_mk2, "3" },
			{ WeaponHash.Compactrifle, "3" },
			{ WeaponHash.Pumpshotgun, "4" },
			{ WeaponHash.Pumpshotgun_mk2, "4" },
			{ WeaponHash.Sawnoffshotgun, "4" },
			{ WeaponHash.Bullpupshotgun, "4" },
			{ WeaponHash.Assaultshotgun, "4" },
		}.ToImmutableDictionary();

		public static readonly ImmutableDictionary<string, List<WeaponHash>> WeaponsByAmmoType = new Dictionary<string, List<WeaponHash>>()
		{
			{
				"1", new List<WeaponHash>()
			{
				WeaponHash.Pistol,
				WeaponHash.Revolver,
				WeaponHash.Revolver_mk2,
				WeaponHash.Pistol_mk2,
				WeaponHash.Combatpistol,
				WeaponHash.Appistol,
				WeaponHash.Pistol50,
				WeaponHash.Snspistol,
				WeaponHash.Snspistol_mk2,
				WeaponHash.Heavypistol,
				WeaponHash.Flaregun,
			}
			},
			{
				"2", new List<WeaponHash>()
			{
				WeaponHash.Microsmg,
				WeaponHash.Smg,
				WeaponHash.Smg_mk2,
				WeaponHash.Assaultsmg,
				WeaponHash.Combatpdw,
				WeaponHash.Machinepistol,
				WeaponHash.Minismg,
			}
			},
			{
				"3", new List<WeaponHash>()
			{
				WeaponHash.Assaultrifle,
				WeaponHash.Assaultrifle_mk2,
				WeaponHash.Carbinerifle,
				WeaponHash.Carbinerifle_mk2,
				WeaponHash.Advancedrifle,
				WeaponHash.Specialcarbine,
				WeaponHash.Bullpuprifle,
				WeaponHash.Bullpuprifle_mk2,
				WeaponHash.Compactrifle,
			}
			},
			{
				"4", new List<WeaponHash>()
			{
				WeaponHash.Pumpshotgun,
				WeaponHash.Pumpshotgun_mk2,
				WeaponHash.Sawnoffshotgun,
				WeaponHash.Bullpupshotgun,
				WeaponHash.Assaultshotgun,
			}
			},
		}.ToImmutableDictionary();

		public static readonly ImmutableDictionary<string, int> PriceByItemName = new Dictionary<string, int>()
		{
			{ "weapon_revolver", 4000 },
			{ "weapon_revolver_mk2", 4500 },
			{ "weapon_pistol_mk2", 1500 },
			{ "weapon_combatpistol", 1000 },
			{ "weapon_appistol", 3500 },
			{ "weapon_pistol50", 5000 },
			{ "weapon_snspistol", 1000 },
			{ "weapon_snspistol_mk2", 1000 },
			{ "weapon_heavypistol", 4000 },
			{ "weapon_flaregun", 1500 },
			{ "weapon_smg", 2000 },
			{ "weapon_smg_mk2", 2000 },
			{ "weapon_assaultsmg", 3000 },
			{ "weapon_combatpdw", 3500 },
			{ "weapon_machinepistol", 3000 },
			{ "weapon_minismg", 2500 },
			{ "weapon_assaultrifle", 4000 },
			{ "weapon_assaultrifle_mk2", 6000 },
			{ "weapon_carbinerifle", 9000 },
			{ "weapon_carbinerifle_mk2", 12000 },
			{ "weapon_advancedrifle", 9500 },
			{ "weapon_specialcarbine", 9000 },
			{ "weapon_bullpuprifle", 8000 },
			{ "weapon_bullpuprifle_mk2", 9500 },
			{ "weapon_compactrifle", 10000 },
			{ "weapon_pumpshotgun_mk2", 2000 },
			{ "weapon_sawnoffshotgun", 3500 },
			{ "weapon_bullpupshotgun", 2500 },
			{ "weapon_assaultshotgun", 5000 },
			{ "1", 500 },
			{ "2", 1000 },
			{ "3", 1000 },
			{ "4", 500 },
			{ "armor", 1000 },
			{ "health", 500 },
		}.ToImmutableDictionary();

		public static readonly ImmutableDictionary<string, int> AmountByAmmoType = new Dictionary<string, int>()
		{
			{ "1", 50 },
			{ "2", 100 },
			{ "3", 80 },
			{ "4", 20 },
		}.ToImmutableDictionary();

		public static List<WeaponHash>? WeaponHashesByAmmoType(string ammoType)
		{
			if (!WeaponsByAmmoType.TryGetValue(ammoType, out List<WeaponHash>? weaponHashes))
			{
				return default;
			}

			return weaponHashes;
		}
	}
}
