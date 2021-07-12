namespace Gamemode.Controllers
{
	using System.Threading.Tasks;
	using Gamemode.ApiClient.Models;
	using Gamemode.Cache.GangWar;
	using Gamemode.Models.Player;
	using Gamemode.Services.Player;
	using GTANetworkAPI;

	public class DeathController : Script
	{
		[ServerEvent(Event.PlayerDeath)]
		private async Task OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
		{
			if (killer == null && reason == 0)
			{
				DeathService.OnPlayerDeath(target, killer, reason, out killer, out reason);
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

			if (target.IsInWarZone && killer.IsInWarZone)
			{
				short delta = killer.Fraction == target.Fraction ? (short)-1 : (short)1;

				GangWarCache.AddKill(killer.Fraction.Value, delta);
				GangWarStats gangWarStats = GangWarCache.GetGangWarStats();
				NAPI.ClientEvent.TriggerClientEventForAll("UpdateGangWarStats", gangWarStats.Ballas, gangWarStats.Bloods, gangWarStats.Marabunta, gangWarStats.Families, gangWarStats.Vagos);
			}

			MoneyService.OnPlayerDeath(target, killer, reason);
			await ExperienceService.OnPlayerDeathAsync(target, killer, reason);
		}
	}
}
