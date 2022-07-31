// <copyright file="Weapon.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using GTANetworkAPI;

	public partial class Weapon
	{
		public Weapon(WeaponHash hash, long amount)
		{
			this.Hash = (long)hash;
			this.Amount = amount;
		}
	}
}
