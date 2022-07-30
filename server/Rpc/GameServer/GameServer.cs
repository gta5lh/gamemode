// <copyright file="GameServer.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.GameServer
{
	using System;
	using System.Collections.Generic;
	using Google.Protobuf;
	using Google.Protobuf.WellKnownTypes;
	using GTANetworkAPI;

	public partial class SetTimeRequest
	{
		public SetTimeRequest(string adminStaticID, string adminName, long hours, long minutes)
		{
			this.AdminStaticID = adminStaticID;
			this.AdminName = adminName;
			this.Hours = hours;
			this.Minutes = minutes;
		}
	}

	public partial class SyncTimeRequest
	{
		public SyncTimeRequest(string adminStaticID, string adminName)
		{
			this.AdminStaticID = adminStaticID;
			this.AdminName = adminName;
		}
	}
}
