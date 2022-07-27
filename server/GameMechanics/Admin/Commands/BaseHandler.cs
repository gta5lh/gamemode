// <copyright file="BaseController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using GTANetworkAPI;

	public class BaseHandler : Script
	{
		protected readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("AdminCommand");
	}
}
