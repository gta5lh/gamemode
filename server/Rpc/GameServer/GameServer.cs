using System;
using System.Collections.Generic;
using Gamemode.Models.Admin;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GTANetworkAPI;

namespace Rpc.GameServer
{
	public partial class SetTimeRequest
	{
		public SetTimeRequest(long adminID, string adminName, long hours, long minutes)
		{
			this.AdminID = adminID;
			this.AdminName = adminName;
			this.Hours = hours;
			this.Minutes = minutes;
		}
	}

	public partial class SyncTimeRequest
	{

		public SyncTimeRequest(long adminID, string adminName)
		{
			this.AdminID = adminID;
			this.AdminName = adminName;
		}
	}
}
