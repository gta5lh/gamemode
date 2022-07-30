// <copyright file="Player.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;

	public partial class UnbanRequest
	{
		public UnbanRequest(string staticID, Guid unbannedByID)
		{
			this.StaticID = staticID;
			this.UnbannedByID = unbannedByID.ToString();
		}
	}
}
