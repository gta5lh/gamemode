// <copyright file="OnGameServerStartRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.GameServer
{
	public partial class OnGameServerStartRequest
	{
		public OnGameServerStartRequest(string serverID)
		{
			this.ServerID = serverID;
		}
	}
}
