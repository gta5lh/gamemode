<<<<<<< Updated upstream
﻿// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.ApiClient.Models;
    using Gamemode.Models.Player;
    using GamemodeCommon.Authentication.Models;
    using GTANetworkAPI;
    using Newtonsoft.Json;

    public class AuthenticationController : Script
    {
        [RemoteEvent("LoginSubmitted")]
        private async Task OnLoginSubmitted(CustomPlayer player, string request)
        {
            if (ResourceStartController.ShouldWait(player.Id))
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
                return;
            }

            LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request);
            List<string> invalidFieldNames = loginRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            User user;

            try
            {
                user = await ApiClient.ApiClient.LoginUser(loginRequest.Email, loginRequest.Password);
            }
            catch (Exception e)
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
                    invalidFieldNames = new List<string>(new string[] { "already_online" });
                    NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                    return;
                }

                CustomPlayer.LoadPlayerCache(player, user);
                NAPI.ClientEvent.TriggerClientEvent(player, "LogIn");
                NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
            });
        }

        [RemoteEvent("RegisterSubmitted")]
        private async Task OnRegisterSubmitted(CustomPlayer player, string request)
        {
            if (ResourceStartController.ShouldWait(player.Id))
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
                return;
            }

            RegisterRequest registerRequest = JsonConvert.DeserializeObject<RegisterRequest>(request);
            List<string> invalidFieldNames = registerRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            User user;

            try
            {
                user = await ApiClient.ApiClient.RegisterUser(registerRequest.Email, registerRequest.Username, registerRequest.Password);
            }
            catch (Exception e)
            {
                invalidFieldNames.Add(e.Message);
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            NAPI.Task.Run(() =>
            {
                CustomPlayer.LoadPlayerCache(player, user);
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
                NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
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
=======
﻿// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.ApiClient.Models;
    using Gamemode.Models.Player;
    using GamemodeCommon.Authentication.Models;
    using GTANetworkAPI;
    using Newtonsoft.Json;

    public class AuthenticationController : Script
    {
        [RemoteEvent("LoginSubmitted")]
        private async Task OnLoginSubmitted(CustomPlayer player, string request)
        {
            if (ResourceStartController.ShouldWait(player.Id))
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
                return;
            }

            LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request);
            List<string> invalidFieldNames = loginRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            User user;

            try
            {
                user = await ApiClient.ApiClient.LoginUser(loginRequest.Email, loginRequest.Password);
            }
            catch (Exception e)
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
                    invalidFieldNames = new List<string>(new string[] { "already_online" });
                    NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                    return;
                }

                CustomPlayer.LoadPlayerCache(player, user);
                NAPI.ClientEvent.TriggerClientEvent(player, "LogIn");
                NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
            });
        }

        [RemoteEvent("RegisterSubmitted")]
        private async Task OnRegisterSubmitted(CustomPlayer player, string request)
        {
            if (ResourceStartController.ShouldWait(player.Id))
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
                return;
            }

            RegisterRequest registerRequest = JsonConvert.DeserializeObject<RegisterRequest>(request);
            List<string> invalidFieldNames = registerRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            User user;

            try
            {
                user = await ApiClient.ApiClient.RegisterUser(registerRequest.Email, registerRequest.Username, registerRequest.Password);
            }
            catch (Exception e)
            {
                invalidFieldNames.Add(e.Message);
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            NAPI.Task.Run(() =>
            {
                CustomPlayer.LoadPlayerCache(player, user);
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
                NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
            });
        }

        [ServerEvent(Event.PlayerDisconnected)]
        private async Task OnPlayerDisconnected(CustomPlayer player, DisconnectionType disconnectType, string reason)
        {
            if (player.LoggedInAt != null)
            {
                await CustomPlayer.UnloadPlayerCache(player);
            }
        }
    }
}
>>>>>>> Stashed changes
