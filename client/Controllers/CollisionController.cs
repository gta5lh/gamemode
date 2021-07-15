// <copyright file="CollisionController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Elements;
	using GamemodeCommon.Models.Data;
	using System.Collections.Generic;
	using System;

	public class CollisionController : Events.Script
	{
		private List<ushort> noCollisionVehicles;
		private DateTime NextCollisionUpdateTime = DateTime.MinValue;
		private const byte UpdateCollisionTimeIntervalSeconds = 5;

		public CollisionController()
		{
			noCollisionVehicles = new List<ushort>();

			Events.AddDataHandler("vehicle_collision_disabled", this.OnVehicleCollisionDisabled);

			Events.OnEntityStreamIn += OnEntityStreamIn;
			Events.OnEntityStreamOut += OnEntityStreamOut;
			Events.Tick += this.OnTick;
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (noCollisionVehicles.Count <= 0 || Entities.Vehicles == null || Entities.Vehicles.Count <= 0) return;

			if (DateTime.UtcNow < this.NextCollisionUpdateTime)
			{
				return;
			}

			List<RAGE.Elements.Vehicle> targetVehicles = new List<Vehicle>();

			foreach (RAGE.Elements.Vehicle vehicle in Entities.Vehicles.Streamed)
			{
				if (!vehicle.Exists) continue;
				if (targetVehicles.Count == noCollisionVehicles.Count) break;

				foreach (ushort vehicleID in noCollisionVehicles)
				{
					if (vehicle.Id != vehicleID) continue;

					targetVehicles.Add(vehicle);
					break;
				}
			}

			foreach (RAGE.Elements.Vehicle target in targetVehicles)
			{
				foreach (RAGE.Elements.Vehicle vehicle in Entities.Vehicles.Streamed)
				{
					if (!vehicle.Exists || vehicle.Id == target.Id) continue;

					target.SetNoCollisionEntity(vehicle.Handle, true);
					vehicle.SetNoCollisionEntity(target.Handle, true);
				}
			}

			Chat.Output($"NoColVeh = {noCollisionVehicles.Count}");
			Chat.Output($"TargetVeh = {targetVehicles.Count}");

			this.NextCollisionUpdateTime = DateTime.UtcNow.AddSeconds(UpdateCollisionTimeIntervalSeconds);
		}

		private void OnVehicleCollisionDisabled(Entity entity, object arg, object oldArg)
		{
			if (entity.Type != RAGE.Elements.Type.Vehicle) return;

			bool vehicleCollisionDisabled = (bool)arg;

			this.UpdateCollision(entity, vehicleCollisionDisabled);
		}

		public void OnEntityStreamIn(RAGE.Elements.Entity entity)
		{
			if (entity.Type != RAGE.Elements.Type.Vehicle) return;
			if (entity.GetSharedData("vehicle_collision_disabled") == null) return;

			bool vehicleCollisionDisabled = (bool)entity.GetSharedData("vehicle_collision_disabled");
			if (!vehicleCollisionDisabled) return;

			this.UpdateCollision(entity, vehicleCollisionDisabled);
		}

		public void OnEntityStreamOut(RAGE.Elements.Entity entity)
		{
			if (noCollisionVehicles.Count <= 0 || entity.Type != RAGE.Elements.Type.Vehicle) return;
			if (entity.GetSharedData("vehicle_collision_disabled") == null) return;

			RAGE.Elements.Vehicle target = (RAGE.Elements.Vehicle)entity;
			noCollisionVehicles.Remove(target.Id);
		}

		private void UpdateCollision(RAGE.Elements.Entity entity, bool vehicleCollisionDisabled)
		{
			RAGE.Elements.Vehicle target = (RAGE.Elements.Vehicle)entity;

			if (Entities.Vehicles != null && Entities.Vehicles.Count > 0)
			{
				foreach (RAGE.Elements.Vehicle vehicle in Entities.Vehicles.Streamed)
				{
					if (!vehicle.Exists || vehicle.Id == target.Id) continue;

					target.SetNoCollisionEntity(vehicle.Handle, !vehicleCollisionDisabled);
					vehicle.SetNoCollisionEntity(target.Handle, !vehicleCollisionDisabled);
				}
			}

			if (vehicleCollisionDisabled)
			{
				if (noCollisionVehicles.Contains(target.Id)) return;
				noCollisionVehicles.Add(target.Id);
			}
			else
			{
				noCollisionVehicles.Remove(target.Id);
			}
		}

		private void Ids()
		{

		}
	}
}
