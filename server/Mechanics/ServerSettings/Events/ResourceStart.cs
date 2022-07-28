// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode.Mechanics.ServerSettings.Events
{
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using Rpc.GameServer;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			NAPI.Server.SetGlobalServerChat(false);
			NAPI.Server.SetAutoSpawnOnConnect(true); // TODO set to false.
			NAPI.Server.SetCommandErrorMessage("Команда не найдена.");

			if (Utils.Environment.IsProduction())
			{
				NAPI.Server.SetLogCommandParamParserExceptions(false);
			}

			Task.WaitAll(OnGameServerStart());
		}

		private static async Task OnGameServerStart()
		{
			try
			{
				await Infrastructure.RpcClients.GameServerService.OnGameServerStartAsync(new OnGameServerStartRequest());
			}
			catch (System.Exception e)
			{
				System.Environment.FailFast("OnGameServerStart failed", e);
			}
		}
	}
}
