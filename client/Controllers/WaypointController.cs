using RAGE;

namespace GamemodeClient.Controllers
{
	public class WaypointController : Events.Script
	{
		private Vector3? waypointPosition;

		public WaypointController()
		{
			RAGE.Events.OnPlayerCreateWaypoint += this.OnWaypointCreated;

			Events.Add("TeleportToWaypoint", this.OnTeleportToWaypoint);
		}

		public void OnWaypointCreated(Vector3 position)
		{
			this.waypointPosition = position;
		}

		public async void OnTeleportToWaypoint(object[] args)
		{
			if (this.waypointPosition == null)
			{
				Chat.Output("Поставь маркер на карте для телепортации");
				return;
			}

			Player.CurrentPlayer.Position = this.waypointPosition;

			float groundZ = 0;
			int tries = 0;

			while (groundZ == 0 && tries <= 3)
			{
				await Task.WaitAsync(250);
				RAGE.Game.Misc.GetGroundZFor3dCoord(this.waypointPosition.X, this.waypointPosition.Y, this.waypointPosition.Z + 2000, ref groundZ, false);
				tries++;
			}

			this.waypointPosition.Z = groundZ;

			Player.CurrentPlayer.Position = this.waypointPosition;
			Chat.Output("Успешно переместились на позицию маркера");
		}
	}
}
