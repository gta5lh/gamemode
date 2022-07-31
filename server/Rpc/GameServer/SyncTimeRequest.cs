// <copyright file="SyncTimeRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.GameServer
{
	public partial class SyncTimeRequest
	{
		public SyncTimeRequest(string adminStaticID, string adminName)
		{
			this.AdminStaticID = adminStaticID;
			this.AdminName = adminName;
		}
	}
}
