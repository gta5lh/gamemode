// <copyright file="RemoveWeaponRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using GTANetworkAPI;

	public partial class RemoveWeaponRequest
	{
		public RemoveWeaponRequest(string staticID, WeaponHash weaponHash, Guid removedByID)
		{
			this.StaticID = staticID;
			this.Hash = (long)weaponHash;
			this.RemovedByID = removedByID.ToString();
		}
	}
}
