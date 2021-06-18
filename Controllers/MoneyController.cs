namespace Gamemode.Controllers
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class MoneyController : Script
    {
        [ServerEvent(Event.PlayerDeath)]
        private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
        {
            if (killer == null && reason == 0)
            {
                List<Player> found = NAPI.Player.GetPlayersInRadiusOfPlayer(3, target);
                foreach (Player player in found)
                {
                    if (player == target) continue;
                    OnPlayerDeath(target, (CustomPlayer)player, (uint)player.CurrentWeapon);
                    return;
                }
            }

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
