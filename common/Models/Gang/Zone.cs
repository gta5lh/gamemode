﻿// <copyright file="Zone.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Models.Gang
{
	public class Zone
	{
		public Zone(long id, long x, long y, long fractionId, bool battleworthy, byte blipColor)
		{
			this.Id = id;
			this.X = x;
			this.Y = y;
			this.FractionId = fractionId;
			this.Battleworthy = battleworthy;
			this.BlipColor = blipColor;
		}

		public long Id { get; set; }

		public long X { get; set; }

		public long Y { get; set; }

		public long FractionId { get; set; }

		public bool Battleworthy { get; set; }

		public byte BlipColor { get; set; }

		public int? BlipId { get; set; }

		public bool IsWarInProgress { get; set; }
	}
}
