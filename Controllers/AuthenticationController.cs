﻿// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.Models.Authentication;
    using Gamemode.Models.Player;
    using Gamemode.Repository;
    using GTANetworkAPI;
    using Newtonsoft.Json;

    public class AuthenticationController : Script
    {
        [RemoteEvent("LoginSubmitted")]
        private async Task OnLoginSubmitted(CustomPlayer player, string request)
        {
            LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request);
            List<string> invalidFieldNames = loginRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            Gamemode.Repositories.Models.User? user = await UserRepository.GetByEmailAndPassword(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                invalidFieldNames = new List<string>(new string[] { "email", "password" });
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            NAPI.Task.Run(() =>
            {
                if (user.BannedUntil != null)
                {
                    invalidFieldNames = new List<string>(new string[] { "banned" });
                    NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                    return;
                }

                if (PlayerUtil.GetByStaticId(user.Id) != null)
                {
                    invalidFieldNames = new List<string>(new string[] { "email" });
                    NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                    return;
                }

                CustomPlayer.LoadPlayerCache(player, user);
                NAPI.ClientEvent.TriggerClientEvent(player, "LogIn");
            });
        }

        [RemoteEvent("RegisterSubmitted")]
        private async Task OnRegisterSubmitted(CustomPlayer player, string request)
        {
            RegisterRequest registerRequest = JsonConvert.DeserializeObject<RegisterRequest>(request);
            List<string> invalidFieldNames = registerRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            Repositories.Models.User user = await UserRepository.GetUserByEmailOrUsername(registerRequest.Email, registerRequest.Username);
            if (user != null)
            {
                if (user.Email == registerRequest.Email)
                {
                    invalidFieldNames.Add("email_exists");
                }

                if (user.Name == registerRequest.Username)
                {
                    invalidFieldNames.Add("username_exists");
                }

                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            Repositories.Models.User newUser = new Repositories.Models.User();
            newUser.Email = registerRequest.Email;
            newUser.Name = registerRequest.Username;
            newUser.Password = registerRequest.Password;

            Repositories.Models.User createdUser = await UserRepository.CreateUser(newUser);
            if (createdUser == null)
            {
                invalidFieldNames = new List<string>(new string[] { "email" });
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            NAPI.Task.Run(() =>
            {
                if (PlayerUtil.GetByStaticId(createdUser.Id) != null)
                {
                    invalidFieldNames = new List<string>(new string[] { "email" });
                    NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                    return;
                }

                CustomPlayer.LoadPlayerCache(player, createdUser);
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
            });
        }

        [ServerEvent(Event.PlayerDisconnected)]
        private async Task OnPlayerDisconnected(CustomPlayer player, DisconnectionType disconnectType, string reason)
        {
            if (IdsCache.DynamicIdByStatic(player.StaticId) != null)
            {
                await CustomPlayer.UnloadPlayerCache(player);
            }
        }
    }
}
