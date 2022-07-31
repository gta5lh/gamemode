// <copyright file="Stats.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.GangWar
{
	public class Stats
	{
		public Stats(long ballas, long bloods, long marabunta, long families, long vagos)
		{
			this.Ballas = ballas;
			this.Bloods = bloods;
			this.Marabunta = marabunta;
			this.Families = families;
			this.Vagos = vagos;
		}

		public long Ballas { get; set; }

		public long Bloods { get; set; }

		public long Marabunta { get; set; }

		public long Families { get; set; }

		public long Vagos { get; set; }
	}
}
