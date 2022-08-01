// <copyright file="ResourceStart.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace Gamemode.Game.ServerSettings.Events
{
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using Rpc.GameServer;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private static void OnResourceStartEx(string resourceName)
		{
			Settings.Init();

			NAPI.Server.SetGlobalServerChat(false);
			NAPI.Server.SetAutoSpawnOnConnect(true);
			NAPI.Server.SetCommandErrorMessage("Команда не найдена.");

			if (Settings.IsProduction())
			{
				NAPI.Server.SetLogCommandParamParserExceptions(false);
			}
		}
	}
}
