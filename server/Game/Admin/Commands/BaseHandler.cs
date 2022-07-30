// <copyright file="BaseHandler.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using GTANetworkAPI;

	public class BaseHandler : Script
	{
		protected readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("AdminCommand");
	}
}
