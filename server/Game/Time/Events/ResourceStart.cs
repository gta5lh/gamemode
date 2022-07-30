// <copyright file="ResourceStart.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace Gamemode.Game.Time.Events
{
	using GTANetworkAPI;

	public class ResourceStart : Script
	{
		[ServerEvent(Event.ResourceStartEx)]
		private static void ResourceStartEx(string resourceName)
		{
			Controllers.Time.InitTimeSyncTimer();
		}
	}
}
