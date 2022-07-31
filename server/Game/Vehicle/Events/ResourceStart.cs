﻿// <copyright file="ResourceStart.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace Gamemode.Game.Vehicle.Events
{
	using Gamemode.Game.Vehicle.Models;
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private static void ResourceStartEx(string resourceName)
		{
			RAGE.Entities.Vehicles.CreateEntity = (NetHandle handle) => new CVehicle(handle);
		}
	}
}
