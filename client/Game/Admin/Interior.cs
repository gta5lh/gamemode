// <copyright file="Interior.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;

	public class Interior : Events.Script
	{
		static Interior()
		{
			Events.Add("TeleportToInterior", OnTeleportToInterior);
		}

		public static void OnTeleportToInterior(object[] args)
		{
			RAGE.Game.Streaming.RequestIpl((string)args[0]);
			Game.Player.Models.Player.CurrentPlayer.Position = new Vector3((float)args[1], (float)args[2], (float)args[3]);
		}
	}
}
