<<<<<<< Updated upstream:Services/Player/ExperienceService.cs
﻿using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
    public class ExperienceService
    {
        public static async void ChangeExperience(CustomPlayer player, short delta)
        {
            short previousExperience = player.CurrentExperience;
            player.CurrentExperience += delta;

            NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", previousExperience, player.CurrentExperience, player.RequiredExperience, player.FractionRank);

            if (player.CurrentExperience >= player.RequiredExperience && player.FractionRank < 10)
            {
                await player.RankUp();

                NAPI.Task.Run(() =>
                {
                    if (player.FractionRank < 10)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", 0, player.CurrentExperience, player.RequiredExperience, player.FractionRank);
                    }

                    NAPI.ClientEvent.TriggerClientEvent(player, "RankedUp", player.FractionRank, player.FractionRankName);

                    NAPI.Task.Run(() =>
                    {
                        player.SendNotification($"Ты повысился до ранга {player.FractionRankName} [{player.FractionRank}]");
                    }, 1500);
                }, 500);
            }

            if (player.CurrentExperience < 0 && player.FractionRank > 1)
            {
                await player.RankDown();

                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", 0, player.CurrentExperience, player.RequiredExperience, player.FractionRank);
                    NAPI.ClientEvent.TriggerClientEvent(player, "RankedDown", player.FractionRank, player.FractionRankName);

                    NAPI.Task.Run(() =>
                    {
                        player.SendNotification($"Ты понизился до ранга {player.FractionRankName} [{player.FractionRank}]");
                    }, 1500);
                }, 500);
            }
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
    public class UserService
    {
        private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("UserService");

        public static async void ChangeExperience(CustomPlayer player, short delta)
        {
            short previousExperience = player.CurrentExperience;
            player.CurrentExperience += delta;

            NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", previousExperience, player.CurrentExperience, player.RequiredExperience, player.FractionRank);

            if (player.CurrentExperience >= player.RequiredExperience && player.FractionRank < 10)
            {
                await player.RankUp();

                NAPI.Task.Run(() =>
                {
                    if (player.FractionRank < 10)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", 0, player.CurrentExperience, player.RequiredExperience, player.FractionRank);
                    }

                    NAPI.ClientEvent.TriggerClientEvent(player, "RankedUp", player.FractionRank, player.FractionRankName);

                    NAPI.Task.Run(() =>
                    {
                        player.SendNotification($"Ты повысился до ранга {player.FractionRankName} [{player.FractionRank}]");
                    }, 1500);
                }, 500);
            }

            if (player.CurrentExperience < 0 && player.FractionRank > 1)
            {
                await player.RankDown();

                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", 0, player.CurrentExperience, player.RequiredExperience, player.FractionRank);
                    NAPI.ClientEvent.TriggerClientEvent(player, "RankedDown", player.FractionRank, player.FractionRankName);

                    NAPI.Task.Run(() =>
                    {
                        player.SendNotification($"Ты понизился до ранга {player.FractionRankName} [{player.FractionRank}]");
                    }, 1500);
                }, 500);
            }
        }

        public static async Task SaveAllUsers()
        {
            List<SaveUserRequest> saveUserRequests = new List<SaveUserRequest>();

            NAPI.Task.Run(() =>
            {
                List<GTANetworkAPI.Player> players = NAPI.Pools.GetAllPlayers();
                if (players == null || players.Count == 0)
                {
                    return;
                }

                foreach (CustomPlayer player in players)
                {
                    saveUserRequests.Add(new SaveUserRequest(player.StaticId, player.CurrentExperience, player.GetAllWeapons(), player.Money));
                }
            });

            await NAPI.Task.WaitForMainThread();

            if (saveUserRequests.Count == 0)
            {
                Logger.Debug("skipping users save: no users online");
                return;
            }

            SaveUsersRequest saveUsersRequest = new SaveUsersRequest(saveUserRequests);

            try
            {
                await ApiClient.ApiClient.SaveUsers(saveUsersRequest);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
>>>>>>> Stashed changes:Services/Player/UserService.cs
