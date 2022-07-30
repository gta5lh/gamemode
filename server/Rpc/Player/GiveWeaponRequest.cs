// <copyright file="GiveWeaponRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using GTANetworkAPI;

	public partial class GiveWeaponRequest
	{
		public GiveWeaponRequest(string staticID, WeaponHash weaponHash, long amount, Guid givenByID)
		{
			this.StaticID = staticID;
			this.Hash = (long)weaponHash;
			this.Amount = amount;
			this.GivenByID = givenByID.ToString();
		}
	}
}
