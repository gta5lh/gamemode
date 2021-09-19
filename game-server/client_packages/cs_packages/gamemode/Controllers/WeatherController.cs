// <copyright file="CollisionController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;

	public class WeatherController : Events.Script
	{
		public WeatherController()
		{
			RAGE.Game.Misc.SetWeatherTypeNowPersist("CLEAR");
			RAGE.Game.Misc.SetWeatherTypePersist("CLEAR");
		}
	}
}
