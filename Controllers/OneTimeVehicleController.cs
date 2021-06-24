using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class OneTimeVehicleController : Script
	{
		[ServerEvent(Event.PlayerDisconnected)]
		private void OnPlayerDisconnected(CustomPlayer player, DisconnectionType type, string reason)
		{
			if (player.OneTimeVehicleId == null)
			{
				return;
			}

			VehicleUtil.GetById(player.OneTimeVehicleId.Value).Delete();
		}
	}
}
