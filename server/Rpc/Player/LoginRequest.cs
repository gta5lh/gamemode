// <copyright file="LoginRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using GTANetworkAPI;

	public partial class LoginRequest
	{
		public LoginRequest(string id, string serverID, string ipAddress, string socialClubID, string computerSerialNumber, GameTypes gameLauncherID)
		{
			this.ID = id;
			this.ServerID = serverID;
			this.IPAddress = ipAddress;
			this.SocialClubID = socialClubID;
			this.ComputerSerialNumber = computerSerialNumber;
			this.GameLauncherID = (long)gameLauncherID;
		}
	}
}
