using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class SpectateController : Script
	{
		[RemoteEvent("TrySpectate")]
		public void TrySpectate(CustomPlayer admin, int id)
		{
			Player found = NAPI.Pools.GetAllPlayers().Find(x => x.Id == id);

			if (found != null) StartSpectate(admin, (CustomPlayer)found);
			else StopSpectate(admin);
		}

		public static void StartSpectate(CustomPlayer admin, CustomPlayer targetPlayer)
		{
			if (admin.SpectatePosition == null) admin.SpectatePosition = admin.Position;
			admin.Spectating = true;
			admin.RemoveAllWeapons();
			admin.Position = targetPlayer.Position + new Vector3(0, 0, 10);
			admin.Dimension = targetPlayer.Dimension;

			admin.TriggerEvent("spectate", targetPlayer.Id);
		}

		public static void StopSpectate(CustomPlayer admin)
		{
			admin.Position = admin.SpectatePosition;
			admin.Dimension = 0;
			admin.Spectating = false;
			admin.SpectatePosition = null;
			admin.TriggerEvent("spectateStop");
		}
	}
}
