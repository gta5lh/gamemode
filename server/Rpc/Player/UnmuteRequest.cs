// <copyright file="UnmuteRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;

	public partial class UnmuteRequest
	{
		public UnmuteRequest(string staticID, Guid unmutedByID)
		{
			this.StaticID = staticID;
			this.UnmutedByID = unmutedByID.ToString();
		}
	}
}
