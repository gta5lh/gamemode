// <copyright file="ResourceStart.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace Gamemode.Game.Player.Events
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private static void ResourceStartEx(string resourceName)
		{
			RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CPlayer(handle);
		}
	}
}
