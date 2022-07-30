// <copyright file="Logger.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Logger
{
	using System.IO;

	public static class Logger
	{
		public static readonly NLog.LogFactory LogFactory = NLog.Web.NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "/dotnet/resources/Gamemode/nlog.config");

		public static readonly NLog.Logger BaseLogger = Gamemode.Logger.Logger.LogFactory.GetLogger("BaseLogger");
	}
}
