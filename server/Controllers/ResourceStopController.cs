// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Gamemode.Commands.Admin;
	using Gamemode.Controllers;
	using Gamemode.Models.Npc;
	using Gamemode.Models.Player;
	using Gamemode.Models.Vehicle;
	using Gamemode.Services;
	using Gamemode.Services.Player;
	using GTANetworkAPI;
	using Microsoft.Extensions.Caching.Memory;
	using NLog.Extensions.Logging;

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
