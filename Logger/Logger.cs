// <copyright file="Logger.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Logger
{
    using System.IO;

    public static class Logger
    {
        public static readonly NLog.LogFactory LogFactory = NLog.Web.NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "/dotnet/resources/Gamemode/nlog.config");
    }
}
