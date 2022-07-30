// <copyright file="InventoryWeapons.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using GTANetworkAPI;

	public class InventoryWeapons
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private readonly ConcurrentDictionary<WeaponHash, bool> inventoryWeapons;

		public InventoryWeapons()
		{
			this.inventoryWeapons = new ConcurrentDictionary<WeaponHash, bool>();
		}

		public bool HasWeapon(WeaponHash weaponHash)
		{
			return this.inventoryWeapons.TryGetValue(weaponHash, out _);
		}

		public void AddWeapon(WeaponHash weaponHash)
		{
			if (this.inventoryWeapons.TryAdd(weaponHash, true))
			{
				Logger.Info($"Added weapon to inventory. weapon_hash={weaponHash}");
			}
		}

		public void RemoveWeapon(WeaponHash weaponHash)
		{
			if (this.inventoryWeapons.TryRemove(weaponHash, out _))
			{
				Logger.Info($"Removed weapon from inventory. weapon_hash={weaponHash}");
			}
		}

		public ICollection<WeaponHash> GetAllWeapons()
		{
			return this.inventoryWeapons.Keys;
		}

		public int Count()
		{
			return this.inventoryWeapons.Count;
		}
	}
}
