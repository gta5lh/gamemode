// <copyright file="MuteRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using Google.Protobuf.WellKnownTypes;

	public partial class MuteRequest
	{
		public MuteRequest(string staticID, string reason, Guid mutedByID, DateTime mutedAt, DateTime mutedUntil)
		{
			this.StaticID = staticID;
			this.MuteReason = reason;
			this.MutedByID = mutedByID.ToString();
			this.MutedAt = Timestamp.FromDateTime(mutedAt);
			this.MutedUntil = Timestamp.FromDateTime(mutedUntil);
		}
	}
}
