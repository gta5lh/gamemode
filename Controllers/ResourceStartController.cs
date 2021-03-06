// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Gamemode.Api;
    using GTANetworkAPI;

    public class ResourceStartController : Script
    {
        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.SetServerSettings();
            Client.InitClient();
        }

        private void SetServerSettings()
        {
            NAPI.Server.SetGlobalServerChat(false);
        }
    }
}
