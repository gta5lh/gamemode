// <copyright file="BanRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using Google.Protobuf.WellKnownTypes;

	public partial class BanRequest
	{
		public BanRequest(string staticID, string reason, Guid bannedByID, DateTime bannedAt, DateTime bannedUntil)
		{
			this.StaticID = staticID;
			this.BanReason = reason;
			this.BannedByID = bannedByID.ToString();
			this.BannedAt = Timestamp.FromDateTime(bannedAt);
			this.BannedUntil = Timestamp.FromDateTime(bannedUntil);
		}
	}
}
