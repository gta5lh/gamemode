// <copyright file="CollisionController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;

	public class HealthController : Events.Script
	{
		public HealthController()
		{
			RAGE.Nametags.Enabled = false;
		}
	}
}
