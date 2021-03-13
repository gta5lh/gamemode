// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class ResourceStartController : Script
    {
        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.SetServerSettings();

            RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CustomPlayer(handle);
        }

        private void SetServerSettings()
        {
            NAPI.Server.SetGlobalServerChat(false);
        }
    }
}
