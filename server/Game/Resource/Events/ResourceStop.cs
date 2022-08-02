// <copyright file="ResourceStop.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Resource.Events
{
	using System.Threading.Tasks;
	using Gamemode.Game.Player;
	using GTANetworkAPI;

	public class ResourceStop : Script
	{
		[ServerEvent(Event.ResourceStop)]
		private void OnResourceStop()
		{
			Task savePlayersOnServerStop = SaveMgr.SavePlayersOnServerStop();

			Task.WaitAll(savePlayersOnServerStop);
		}
	}
}
