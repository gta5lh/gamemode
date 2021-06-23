namespace Gamemode.Controllers
{
	using Gamemode.Models.Player;
	using Gamemode.Services.Player;
	using GTANetworkAPI;
	using System.Collections.Generic;

	public class DeathController : Script
	{
		[ServerEvent(Event.PlayerDeath)]
		private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
		{
			if (killer == null && reason == 0)
			{
				List<Player> found = NAPI.Player.GetPlayersInRadiusOfPlayer(3, target);
				foreach (CustomPlayer player in found)
				{
					if (player == target) continue;
					killer = player;
					reason = (uint)player.CurrentWeapon;
					break;
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

			ExperienceService.OnPlayerDeath(target, killer, reason);
			MoneyService.OnPlayerDeath(target, killer, reason);
		}
	}
}
