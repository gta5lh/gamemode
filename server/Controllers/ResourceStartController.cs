﻿// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Cache.GangZone;
	using Gamemode.Commands.Admin;
	using Gamemode.Controllers;
	using Gamemode.Models.Npc;
	using Gamemode.Models.Player;
	using Gamemode.Models.Vehicle;
	using GTANetworkAPI;
	using Microsoft.Extensions.Caching.Memory;
	using NLog.Extensions.Logging;
	using Rollbar;
	using Rpc.GameServer;

	public class ResourceStartController : Script
	{
		private static IMemoryCache Cache;

		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			ExceptionController.InitRollbar();
			this.SetServerSettings();
			SpawnNpcs.CreateSpawnNpcs();

			RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CustomPlayer(handle);
			RAGE.Entities.Vehicles.CreateEntity = (NetHandle handle) => new CustomVehicle(handle);

			Cache = new MemoryCache(new MemoryCacheOptions { }, new NLogLoggerFactory());
			TimeController.InitTimeSyncTimer();
			WeatherController.InitWeatherSyncTimer();
			SavePlayersController.InitSavePlayerTimer();

			Task initGangZoneCacheTask = GangZoneCache.InitGangZoneCache();
			Task onGameServerStartTask = OnGameServerStart();
			Task startGangWarJobsTask = GangWarController.StartGangWarJobs();
			Task payDayJobTask = PayDayController.StartPayDayJob();

			Task.WaitAll(initGangZoneCacheTask, onGameServerStartTask, startGangWarJobsTask, payDayJobTask);
		}

		private void SetServerSettings()
		{
			NAPI.Server.SetGlobalServerChat(false);
			NAPI.Server.SetAutoSpawnOnConnect(false);
			NAPI.Server.SetCommandErrorMessage("Команда не найдена.");

			if (Utils.Environment.IsProduction())
			{
				NAPI.Server.SetLogCommandParamParserExceptions(false);
			}
		}

		public static bool ShouldWait(ushort playerId)
		{
			string cacheKey = $"{CacheKeys.PlayerAuthenticationAction}{playerId}";

			if (Cache.Get<bool?>(cacheKey) != null)
			{
				return true;
			}

			var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3));
			Cache.Set(cacheKey, true, cacheEntryOptions);

			return false;
		}

		private static async Task OnGameServerStart()
		{
			try
			{
				await Infrastructure.RpcClients.GameServerService.OnGameServerStartAsync(new OnGameServerStartRequest());
			}
			catch
			{
			}
		}
	}

	public static class CacheKeys
	{
		public static string PlayerAuthenticationAction { get { return "PlayerAuthenticationAction"; } }
	}
}
