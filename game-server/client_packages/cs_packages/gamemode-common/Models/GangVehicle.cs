// <copyright file="GangVehicle.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Models
{
	public class GangVehicle
	{
		public GangVehicle(uint model, byte rank)
		{
			this.Model = model;
			this.Rank = rank;
		}

		public uint Model { get; set; }

		public byte Rank { get; set; }
	}
}
