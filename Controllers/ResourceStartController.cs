// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using Gamemode.Models.Player;
    using Gamemode.Models.Settings;
    using Gamemode.Repository;
    using GTANetworkAPI;

    public class ResourceStartController : Script
    {
        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.SetServerSettings();

            var u = new UserDatabaseSettings();
            u.UsersCollectionName = "users";
            u.StaticIdsCollectionName = "static_ids";
            u.ConnectionString = "mongodb://localhost:27017";
            u.DatabaseName = "gta";

            UserRepository.InitUserRepository(u);

            RAGE.Entities.Players.CreateEntity = (NetHandle handle) => new CustomPlayer(handle);
        }

        private void SetServerSettings()
        {
            NAPI.Server.SetGlobalServerChat(false);
        }
    }
}
