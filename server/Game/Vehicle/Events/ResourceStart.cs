// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode.Game.Vehicle.Events
{
	using Gamemode.Game.Vehicle.Models;
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			RAGE.Entities.Vehicles.CreateEntity = (NetHandle handle) => new CVehicle(handle);
		}
	}
}
