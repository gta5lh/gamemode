// <copyright file="Waypoint.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace GamemodeClient.Controllers
{
	using System.Diagnostics.CodeAnalysis;
	using GamemodeClient.Game.Player.Models;
	using RAGE;

	public class Waypoint : Events.Script
	{
		private static Vector3? waypointPosition;

		static Waypoint()
		{
			Events.OnPlayerCreateWaypoint += OnWaypointCreated;

			Events.Add("TeleportToWaypoint", OnTeleportToWaypoint);
		}

		public static void OnWaypointCreated(Vector3 position)
		{
			waypointPosition = position;
		}

		[SuppressMessage("Usage", "AsyncFixer03:Avoid unsupported fire-and-forget async-void methods or delegates. Unhandled exceptions will crash the process", Justification = "It's fine here")]
		public static async void OnTeleportToWaypoint(object[] args)
		{
			if (waypointPosition == null)
			{
				Chat.Output("Поставь маркер на карте для телепортации");
				return;
			}

			Player.CurrentPlayer.Position = waypointPosition;

			float groundZ = 0;
			for (int tries = 0; groundZ == 0 && tries <= 3; tries++)
			{
				await Task.WaitAsync(250).ConfigureAwait(false);
				RAGE.Game.Misc.GetGroundZFor3dCoord(waypointPosition.X, waypointPosition.Y, waypointPosition.Z + 2000, ref groundZ, false);
			}

			waypointPosition.Z = groundZ;

			Player.CurrentPlayer.Position = waypointPosition;
			Chat.Output("Успешно переместились на позицию маркера");
		}
	}
}
