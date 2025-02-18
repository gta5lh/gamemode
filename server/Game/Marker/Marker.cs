﻿// <copyright file="Marker.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Marker
{
	using GTANetworkAPI;

	public class Marker
	{
		private readonly Vector3 position;
		private readonly Color color;
		private readonly MarkerType markerType;
		private readonly string text;
		private readonly Colshape.IColShapeEventHandler colShapeEnterEvent;

		public Marker(Vector3 position, Color color, MarkerType markerType, string text, Colshape.IColShapeEventHandler colShapeEnterEvent)
		{
			this.position = position;
			this.color = color;
			this.markerType = markerType;
			this.text = text;
			this.colShapeEnterEvent = colShapeEnterEvent;
		}

		public void Create()
		{
			NAPI.Marker.CreateMarker(this.markerType, this.position, new Vector3(0, 1, 0), new Vector3(0, 1, 0), 1f, this.color, true);
			NAPI.TextLabel.CreateTextLabel(this.text, this.position, 20, 12, 0, this.color);
			ColShape colShape = NAPI.ColShape.CreateCylinderColShape(this.position, 1, 1);
			colShape.OnEntityEnterColShape += this.colShapeEnterEvent.OnEntityEnterColShape;
			colShape.OnEntityExitColShape += this.colShapeEnterEvent.OnEntityExitColShape;
		}
	}
}
