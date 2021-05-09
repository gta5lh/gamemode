namespace Gamemode.Controllers
{
    using Gamemode.Models.Player;
    using Gamemode.Services.Player;
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

            short delta = killer.Fraction == target.Fraction ? (short)-1 : (short)1;

            ExperienceService.ChangeExperience(killer, delta);
        }
    }
}
