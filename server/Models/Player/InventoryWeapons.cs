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

		public void AddWeapon(WeaponHash weaponHash)
		{
			if (this.inventoryWeapons.TryAdd(weaponHash, true))
			{
				logger.Info($"Added weapon to inventory. weapon_hash={weaponHash}");
			}
		}

		public void RemoveWeapon(WeaponHash weaponHash)
		{
			if (this.inventoryWeapons.TryRemove(weaponHash, out _))
			{
				logger.Info($"Removed weapon from inventory. weapon_hash={weaponHash}");
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
