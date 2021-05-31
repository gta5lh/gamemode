using Gamemode.Models.Player;
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
