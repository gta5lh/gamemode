// <copyright file="SetAdminRankRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using Gamemode.Game.Admin.Models;

	public partial class SetAdminRankRequest
	{
		public SetAdminRankRequest(string staticID, AdminRank rank, Guid setByID)
		{
			this.StaticID = staticID;
			this.Rank = (long)rank;
			this.SetByID = setByID.ToString();
		}
	}
}
