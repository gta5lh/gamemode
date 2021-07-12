// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
	using System.Threading.Tasks;
	using Gamemode.Controllers;
	using Gamemode.Services;
	using Gamemode.Services.Player;
	using GTANetworkAPI;

	public class ResourceStopController : Script
	{
		[ServerEvent(Event.ResourceStop)]
		private void ResourceStop()
		{
			Task finishGangWarAsFailed = GangWarService.FinishGangWarAsFailed();
			Task stopGangWarJobs = GangWarController.StopGangWarJobs();
			Task savePlayersOnServerStop = PlayerService.SavePlayersOnServerStop();

			Task.WaitAll(finishGangWarAsFailed, stopGangWarJobs, savePlayersOnServerStop);
		}
	}
}
