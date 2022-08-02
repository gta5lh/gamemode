// <copyright file="ResourceStart.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Resource.Events
{
	using System.Threading.Tasks;
	using Gamemode.Game.Exception;
	using Gamemode.Game.Player;
	using Gamemode.Game.ServerSettings;
	using GTANetworkAPI;
	using Rpc.GameServer;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private static void OnResourceStartEx(string resourceName)
		{
			Exception.InitRollbar();

			Task initGangZoneCacheTask = GangZone.Cache.Init();
			Task onGameServerStartTask = OnGameServerStart();

			Task.WaitAll(initGangZoneCacheTask, onGameServerStartTask);

			SaveMgr.InitSavePlayerTimer();
		}

		private static async Task OnGameServerStart()
		{
			try
			{
				await Infrastructure.RpcClients.GameServerService.OnGameServerStartAsync(new OnGameServerStartRequest(Settings.ServerID));
			}
			catch (System.Exception e)
			{
				System.Environment.FailFast("OnGameServerStart failed", e);
			}
		}
	}
}
