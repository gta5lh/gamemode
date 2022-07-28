﻿// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode.Mechanics.Weather.Events
{
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			Controllers.Weather.InitWeatherSyncTimer();
		}
	}
}
