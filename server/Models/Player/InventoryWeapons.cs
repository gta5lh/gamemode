using System.Collections.Concurrent;
using System.Collections.Generic;
using GTANetworkAPI;

namespace Gamemode.Models.Player
{
	public class InventoryWeapons
	{
		private static readonly NLog.ILogger logger = Logger.Logger.LogFactory.GetCurrentClassLogger();
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
			this.inventoryWeapons.TryAdd(weaponHash, true);
		}

		public void RemoveWeapon(WeaponHash weaponHash)
		{
			this.inventoryWeapons.TryRemove(weaponHash, out _);
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
