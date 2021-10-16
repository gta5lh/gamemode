using System.Collections.Generic;
using Gamemode.Controllers;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
	public class DeathService
	{
		public static void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason, out CustomPlayer foundKiller, out uint foundReason)
		{
			foundKiller = killer;
			foundReason = reason;

			List<GTANetworkAPI.Player> found = NAPI.Player.GetPlayersInRadiusOfPlayer(3, target);
			foreach (CustomPlayer player in found)
			{
				if (player == target || player.Invisible || player.Noclip || player.Spectating) continue;
				foundKiller = player;
				foundReason = (uint)player.CurrentWeapon;
				return;
			}
		}
	}
}
