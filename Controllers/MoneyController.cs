namespace Gamemode.Controllers
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class MoneyController : Script
    {
        [ServerEvent(Event.PlayerDeath)]
        private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
        {
            if (killer == null || killer == target)
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

            long reward = GangUtil.RewardByRank[target.FractionRank.Value];

            if (killer.Fraction == target.Fraction)
            {
                killer.Money -= reward;
            }
            else
            {
                killer.Money += reward;
            }
        }
    }
}
