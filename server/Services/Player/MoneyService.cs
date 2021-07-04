using Gamemode.Models.Player;

namespace Gamemode.Services.Player
{
	public class MoneyService
	{
		public static async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
		{
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
