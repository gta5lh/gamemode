// <copyright file="Npc.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Npc
{
	using System;
	using Gamemode.Game.Colshape;
	using GTANetworkAPI;

	public class Npc
	{
		public Npc(Vector3 position, float heading, string name, PedHash model, IColShapeEventHandler colShapeEventHandler, int blip, string blipName)
		{
			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
			this.ColShapeEventHandler = colShapeEventHandler;
			this.Blip = blip;
			this.BlipName = blipName;
		}

		public Npc(Vector3 position, float heading, string name, PedHash model, IColShapeEventHandler colShapeEventHandler)
		{
			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
			this.ColShapeEventHandler = colShapeEventHandler;
		}

		public Npc(Vector3 position, float heading, string name, PedHash model)
		{
			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
		}

		public Vector3 Position { get; }

		public float Heading { get; }

		public string Name { get; }

		public PedHash Model { get; }

		public int? Blip { get; }

		public string? BlipName { get; }

		public IColShapeEventHandler ColShapeEventHandler { get; }

		public void Create()
		{
			NAPI.Ped.CreatePed((uint)this.Model, this.Position, this.Heading, false, true, true);

			if (this.ColShapeEventHandler != null)
			{
				ColShape colshape = NAPI.ColShape.CreateCylinderColShape(this.Position, 2, 1);
				colshape.OnEntityEnterColShape += this.ColShapeEventHandler.OnEntityEnterColShape;
				colshape.OnEntityExitColShape += this.ColShapeEventHandler.OnEntityExitColShape;
			}

			if (this.Blip != null)
			{
				NAPI.Blip.CreateBlip(this.Blip.Value, this.Position, 1, 0, this.BlipName, 255, 0, true);
			}
		}
	}
}
