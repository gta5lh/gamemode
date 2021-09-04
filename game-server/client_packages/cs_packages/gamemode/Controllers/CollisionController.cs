// <copyright file="CollisionController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeCommon.Models.Data;
	using RAGE;
	using RAGE.Elements;

	public class CollisionController : Events.Script
	{
		public CollisionController()
		{
			Events.AddDataHandler(DataKey.VehicleCollisionDisabled, this.OnVehicleCollisionDisabled);
			Events.OnEntityStreamIn += OnEntityStreamIn;
		}

		private void OnVehicleCollisionDisabled(Entity entity, object arg, object oldArg)
		{
			if (entity.Type != RAGE.Elements.Type.Vehicle || !Player.IsInVehicle() || entity.Id != Player.CurrentPlayer.Vehicle.Id) return;

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
			if (!Player.IsInVehicle() || Player.CurrentPlayer.Vehicle.GetSharedData(DataKey.VehicleCollisionDisabled) == null || !(bool)Player.CurrentPlayer.Vehicle.GetSharedData(DataKey.VehicleCollisionDisabled)) return;

			RAGE.Elements.Vehicle target = (RAGE.Elements.Vehicle)entity;
			target.SetNoCollisionEntity(Player.CurrentPlayer.Vehicle.Handle, false);
		}
	}
}
