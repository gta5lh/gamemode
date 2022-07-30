// <copyright file="GameServer.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.GameServer
{
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
}
