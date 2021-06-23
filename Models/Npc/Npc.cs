using System;
using Gamemode.Colshape;
using GTANetworkAPI;

namespace Gamemode.Models.Npc
{
	public class Npc
	{
		public Vector3 Position { get; }
		public float Heading { get; }
		public string Name { get; }
		public PedHash Model { get; }

		public int? Blip { get; }
		public string? BlipName { get; }

		public IColShapeEventHandler colShapeEventHandler { get; }

		public Npc(Vector3 position, float heading, string name, PedHash model, IColShapeEventHandler colShapeEventHandler, int blip, string blipName)
		{
			if (colShapeEventHandler == null) throw new ArgumentNullException("colshapeEnterEvent");

			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
			this.colShapeEventHandler = colShapeEventHandler;
			this.Blip = blip;
			this.BlipName = blipName;
		}

		public Npc(Vector3 position, float heading, string name, PedHash model, IColShapeEventHandler colShapeEventHandler)
		{
			if (colShapeEventHandler == null) throw new ArgumentNullException("colshapeEnterEvent");

			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
			this.colShapeEventHandler = colShapeEventHandler;
		}
		public Npc(Vector3 position, float heading, string name, PedHash model)
		{
			this.Position = position;
			this.Heading = heading;
			this.Name = name;
			this.Model = model;
		}

		public void Create()
		{
			NAPI.Ped.CreatePed((uint)this.Model, this.Position, this.Heading, false, true, true);

			if (this.colShapeEventHandler != null)
			{
				ColShape colshape = NAPI.ColShape.CreateCylinderColShape(this.Position, 2, 1);
				colshape.OnEntityEnterColShape += this.colShapeEventHandler.OnEntityEnterColShape;
				colshape.OnEntityExitColShape += this.colShapeEventHandler.OnEntityExitColShape;
			}

			if (this.Blip != null)
			{
				NAPI.Blip.CreateBlip(this.Blip.Value, this.Position, 1, 0, this.BlipName, 255, 0, true);
			}
		}
	}
}
