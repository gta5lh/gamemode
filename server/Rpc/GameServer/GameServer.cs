using System;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GTANetworkAPI;

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

	public partial class SyncTimeRequest
	{

		public SyncTimeRequest(string adminStaticID, string adminName)
		{
			this.AdminStaticID = adminStaticID;
			this.AdminName = adminName;
		}
	}
}
