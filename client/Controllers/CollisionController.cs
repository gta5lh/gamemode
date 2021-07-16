﻿// <copyright file="CollisionController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Elements;

	public class CollisionController : Events.Script
	{
		public CollisionController()
		{
			Events.AddDataHandler("vehicle_collision_disabled", this.OnVehicleCollisionDisabled);
			Events.OnEntityStreamIn += OnEntityStreamIn;
		}

		private void OnVehicleCollisionDisabled(Entity entity, object arg, object oldArg)
		{
			if (entity.Type != RAGE.Elements.Type.Vehicle || Player.CurrentPlayer.Vehicle == null || !Player.CurrentPlayer.Vehicle.Exists || entity.Id != Player.CurrentPlayer.Vehicle.Id) return;

			bool vehicleCollisionDisabled = (bool)arg;

			RAGE.Elements.Vehicle target = (RAGE.Elements.Vehicle)entity;

			if (Entities.Vehicles != null && Entities.Vehicles.Count > 0)
			{
				foreach (RAGE.Elements.Vehicle vehicle in Entities.Vehicles.Streamed)
				{
					if (!vehicle.Exists || vehicle.Id == target.Id) continue;

					vehicle.SetNoCollisionEntity(target.Handle, !vehicleCollisionDisabled);
				}
			}
		}

		public void OnEntityStreamIn(RAGE.Elements.Entity entity)
		{
			if (entity.Type != RAGE.Elements.Type.Vehicle) return;
			if (Player.CurrentPlayer.Vehicle == null || !Player.CurrentPlayer.Vehicle.Exists || Player.CurrentPlayer.Vehicle.GetSharedData("vehicle_collision_disabled") == null || !(bool)Player.CurrentPlayer.Vehicle.GetSharedData("vehicle_collision_disabled")) return;

			RAGE.Elements.Vehicle target = (RAGE.Elements.Vehicle)entity;
			target.SetNoCollisionEntity(Player.CurrentPlayer.Vehicle.Handle, false);
		}
	}
}
