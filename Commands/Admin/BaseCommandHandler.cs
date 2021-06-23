// <copyright file="BaseController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
	using GTANetworkAPI;

	public class BaseCommandHandler : Script
	{
		protected readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("AdminCommand");
	}
}
