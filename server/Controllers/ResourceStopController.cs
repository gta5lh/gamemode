// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Gamemode.Commands.Admin;
    using Gamemode.Controllers;
    using Gamemode.Models.Npc;
    using Gamemode.Models.Player;
    using Gamemode.Models.Vehicle;
    using Gamemode.Services;
    using GTANetworkAPI;
    using Microsoft.Extensions.Caching.Memory;
    using NLog.Extensions.Logging;

    public class ResourceStopController : Script
    {
        private static IMemoryCache Cache;

        [ServerEvent(Event.ResourceStop)]
        private void ResourceStop()
        {
            Task finishGangWarAsFailed = GangWarService.FinishGangWarAsFailed();
            Task stopGangWarJobs = GangWarController.StopGangWarJobs();

            Task.WaitAll(finishGangWarAsFailed, stopGangWarJobs);
        }
    }
}
