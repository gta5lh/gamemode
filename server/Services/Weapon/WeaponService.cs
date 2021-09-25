using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Rpc.User;

namespace Gamemode.Services
{
	public static class WeaponService
	{
		public static List<WeaponHash>? WeaponHashesByAmmoType(string ammoType)
		{
			List<WeaponHash>? weaponHashes;
			if (!WeaponsByAmmoType.TryGetValue(ammoType, out weaponHashes))
			{
				return null;
			}

			return weaponHashes;
		}

		public static readonly Dictionary<WeaponHash, string> AmmoTypeByWeapon = new Dictionary<WeaponHash, string>(){
			{ WeaponHash.Pistol_mk2, "1" },
			{ WeaponHash.Combatpistol, "1" },
			{ WeaponHash.Appistol, "1" },
			{ WeaponHash.Pistol50, "1" },
			{ WeaponHash.Snspistol, "1" },
			{ WeaponHash.Snspistol_mk2, "1" },
			{ WeaponHash.Heavypistol, "1" },
			{ WeaponHash.Flaregun, "1" },

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

			{ WeaponHash.Pumpshotgun_mk2, "4" },
			{ WeaponHash.Sawnoffshotgun, "4" },
			{ WeaponHash.Bullpupshotgun, "4" },
			{ WeaponHash.Assaultshotgun, "4" },
		};

		public static readonly Dictionary<string, List<WeaponHash>> WeaponsByAmmoType = new Dictionary<string, List<WeaponHash>>(){
			{"1", new List<WeaponHash>() {
				WeaponHash.Pistol_mk2,
				WeaponHash.Combatpistol,
				WeaponHash.Appistol,
				WeaponHash.Pistol50,
				WeaponHash.Snspistol,
				WeaponHash.Snspistol_mk2,
				WeaponHash.Heavypistol,
				WeaponHash.Flaregun,
			}},
			{"2", new List<WeaponHash>() {
				WeaponHash.Smg,
				WeaponHash.Smg_mk2,
				WeaponHash.Assaultsmg,
				WeaponHash.Combatpdw,
				WeaponHash.Machinepistol,
				WeaponHash.Minismg,
			}},
			{"3", new List<WeaponHash>() {
				WeaponHash.Assaultrifle,
				WeaponHash.Assaultrifle_mk2,
				WeaponHash.Carbinerifle,
				WeaponHash.Carbinerifle_mk2,
				WeaponHash.Advancedrifle,
				WeaponHash.Specialcarbine,
				WeaponHash.Bullpuprifle,
				WeaponHash.Bullpuprifle_mk2,
				WeaponHash.Compactrifle,
			}},
			{"4", new List<WeaponHash>() {
				WeaponHash.Pumpshotgun_mk2,
				WeaponHash.Sawnoffshotgun,
				WeaponHash.Bullpupshotgun,
				WeaponHash.Assaultshotgun,
			}}
		};

		public static readonly Dictionary<string, int> PriceByItemName = new Dictionary<string, int>(){
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
		};

		public static readonly Dictionary<string, int> AmountByAmmoType = new Dictionary<string, int>(){
			{ "1", 50 },
			{ "2", 100 },
			{ "3", 80 },
			{ "4", 20 },
		};
	}
}
