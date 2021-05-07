namespace Gamemode.Controllers
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class ExperienceController : Script
    {
        [ServerEvent(Event.PlayerDeath)]
        private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
        {
            if (killer == null)
            {
                return;
            }

            if (target.Fraction == null)
            {
                // TODO: Punish if newby killed?
                return;
            }

            if (killer.Fraction == null)
            {
                // TODO: Punish if newby killed gang member?
                return;
            }

            short previousExperience = killer.CurrentExperience;

            if (killer.Fraction == target.Fraction)
            {
                killer.CurrentExperience--;
            }
            else
            {
                killer.CurrentExperience++;
            }

            NAPI.ClientEvent.TriggerClientEvent(killer, "ExperienceChanged", previousExperience, killer.CurrentExperience, killer.RequiredExperience, killer.FractionRank);

            if (killer.CurrentExperience >= killer.RequiredExperience && killer.FractionRank < 10)
            {
                await killer.RankUp();

                NAPI.Task.Run(() =>
                {
                    if (killer.FractionRank < 10)
                    {
                        NAPI.ClientEvent.TriggerClientEvent(killer, "ExperienceChanged", 0, killer.CurrentExperience, killer.RequiredExperience, killer.FractionRank);
                    }

                    NAPI.ClientEvent.TriggerClientEvent(killer, "RankedUp", killer.FractionRank, killer.FractionRankName);

                    NAPI.Task.Run(() =>
                    {
                        killer.SendNotification($"Ты повысился до ранга {killer.FractionRankName} [{killer.FractionRank}]");
                    }, 1500);
                }, 500);
            }

            if (killer.CurrentExperience < 0 && killer.FractionRank > 1)
            {
                await killer.RankDown();

                NAPI.Task.Run(() =>
                {
                    NAPI.ClientEvent.TriggerClientEvent(killer, "ExperienceChanged", 0, killer.CurrentExperience, killer.RequiredExperience, killer.FractionRank);
                    NAPI.ClientEvent.TriggerClientEvent(killer, "RankedDown", killer.FractionRank, killer.FractionRankName);

                    NAPI.Task.Run(() =>
                        {
                            killer.SendNotification($"Ты понизился до ранга {killer.FractionRankName} [{killer.FractionRank}]");
                        }, 1500);
                }, 500);
            }
        }
    }
}
