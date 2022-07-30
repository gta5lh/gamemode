// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode.Game.Player.Events
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CPlayer(handle);
		}
	}
}
