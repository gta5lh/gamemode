// <copyright file="GangUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using Rpc.User;

namespace Gamemode
{
	public static class GangUtil
	{
		public static readonly byte NpcIdBloods = 1;
		public static readonly byte NpcIdBallas = 2;
		public static readonly byte NpcIdTheFamilies = 3;
		public static readonly byte NpcIdVagos = 4;
		public static readonly byte NpcIdMarabunta = 5;

		public static readonly Dictionary<long, string> GangNameById = new Dictionary<long, string>()
		{
			{ NpcIdBloods, "bloods" },
			{ NpcIdBallas, "ballas"},
			{ NpcIdTheFamilies, "the_families"},
			{ NpcIdVagos, "vagos" },
			{ NpcIdMarabunta, "marabunta"},
		};

		public static readonly Dictionary<string, long> GangIdByName = new Dictionary<string, long>()
		{
			{ "bloods", NpcIdBloods},
			{ "ballas", NpcIdBallas},
			{ "the_families", NpcIdTheFamilies},
			{ "vagos", NpcIdVagos},
			{ "marabunta", NpcIdMarabunta},
		};

		public static readonly Dictionary<string, Color> GangColorByName = new Dictionary<string, Color>()
		{
			{ "ballas", Ballas.Color},
			{ "bloods", Bloods.Color},
			{ "marabunta", Marabunta.Color},
			{ "the_families", TheFamilies.Color},
			{ "vagos", Vagos.Color},
		};

		public static readonly Dictionary<long, Spawn> CarSpawnByGangId = new Dictionary<long, Spawn>()
		{
			{ NpcIdBallas, Ballas.CarSpawn},
			{ NpcIdBloods, Bloods.CarSpawn},
			{ NpcIdMarabunta, Marabunta.CarSpawn},
			{ NpcIdTheFamilies, TheFamilies.CarSpawn},
			{ NpcIdVagos, Vagos.CarSpawn},
		};

		public static readonly Dictionary<long, long> RewardByRank = new Dictionary<long, long>()
		{
			{ 1, 10},
			{ 2, 20},
			{ 3, 30},
			{ 4, 40},
			{ 5, 50},
			{ 6, 60},
			{ 7, 70},
			{ 8, 80},
			{ 9, 90},
			{ 10, 100},
		};

		public static readonly Dictionary<long, long> SalaryByRank = new Dictionary<long, long>()
		{
			{ 1, 400},
			{ 2, 500},
			{ 3, 600},
			{ 4, 700},
			{ 5, 800},
			{ 6, 900},
			{ 7, 1000},
			{ 8, 1500},
			{ 9, 2000},
			{ 10, 2500},
		};

		public static readonly Dictionary<long, ICollection<Weapon>> WeaponsByGangId = new Dictionary<long, ICollection<Weapon>>()
		{
			{ NpcIdBallas, new Weapon[]{
				new Weapon(WeaponHash.Flare, 3),
				new Weapon(WeaponHash.Bat, 1),
				new Weapon(WeaponHash.Pistol, 100),
				new Weapon(WeaponHash.Microsmg, 100),
				new Weapon(WeaponHash.Pumpshotgun, 10),
			}},
			{ NpcIdVagos, new Weapon[]{
				new Weapon(WeaponHash.Flare, 3),
				new Weapon(WeaponHash.Crowbar, 1),
				new Weapon(WeaponHash.Pistol, 100),
				new Weapon(WeaponHash.Microsmg, 100),
				new Weapon(WeaponHash.Pumpshotgun, 10),
			}},
			{ NpcIdTheFamilies, new Weapon[]{
				new Weapon(WeaponHash.Flare, 3),
				new Weapon(WeaponHash.Knife, 1),
				new Weapon(WeaponHash.Pistol, 100),
				new Weapon(WeaponHash.Microsmg, 100),
				new Weapon(WeaponHash.Pumpshotgun, 10),
			}},
			{ NpcIdMarabunta, new Weapon[]{
				new Weapon(WeaponHash.Flare, 3),
				new Weapon(WeaponHash.Knuckle, 1),
				new Weapon(WeaponHash.Pistol, 100),
				new Weapon(WeaponHash.Microsmg, 100),
				new Weapon(WeaponHash.Pumpshotgun, 10),
			}},
			{ NpcIdBloods, new Weapon[]{
				new Weapon(WeaponHash.Flare, 3),
				new Weapon(WeaponHash.Hatchet, 1),
				new Weapon(WeaponHash.Pistol, 100),
				new Weapon(WeaponHash.Microsmg, 100),
				new Weapon(WeaponHash.Pumpshotgun, 10),
			}},
		};

		public static readonly Dictionary<long, byte> BlipColorByGangId = new Dictionary<long, byte>()
		{
			{ NpcIdBloods, Bloods.BlipColor },
			{ NpcIdBallas, Ballas.BlipColor},
			{ NpcIdTheFamilies, TheFamilies.BlipColor},
			{ NpcIdVagos, Vagos.BlipColor },
			{ NpcIdMarabunta, Marabunta.BlipColor},
		};

		public static readonly Dictionary<long, string> VehiclesByRankId = new Dictionary<long, string>()
		{
			{ 2, "Manana, Emperor" },
			{ 3, "Voodoo"},
			{ 4, "Picador, Moonbeam"},
			{ 5, "Hexer, Sovereign" },
			{ 6, "Yosemite, Faction, Warrener"},
			{ 7, "Buccaneer, Ruiner"},
			{ 8, "Riata, Cognoscenti"},
			{ 9, "Gauntlet"},
			{ 10, "Superd, Dubsta, Dominator"},
		};

		public const string BallasName = "Ballas";
		public const string BloodsName = "Bloods";
		public const string MarabuntaName = "Marabunta";
		public const string TheFamiliesName = "TheFamilies";
		public const string VagosName = "Vagos";

		public static string ChatColorFromGangName(string gangName)
		{
			switch (gangName)
			{
				case BallasName:
					return "~p~";

				case BloodsName:
					return "~q~";

				case MarabuntaName:
					return "~b~";

				case TheFamiliesName:
					return "~g~";

				case VagosName:
					return "~y~";
			}

			return string.Empty;
		}
	}
}
