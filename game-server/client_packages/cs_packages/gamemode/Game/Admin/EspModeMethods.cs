// <copyright file="EspModeMethods.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	public static class EspModeMethods
	{
		public static string DisplayMode(this EspMode espMode)
		{
			switch (espMode)
			{
				case EspMode.OnlyPlayers:
					return "только игроки";

				case EspMode.PlayersWithCars:
					return "игроки с машинами";

				default:
					return "выключен";
			}
		}

		public static EspMode IncreaseEspMode(this EspMode espMode)
		{
			switch (espMode)
			{
				case EspMode.OnlyPlayers:
					return EspMode.PlayersWithCars;

				case EspMode.PlayersWithCars:
					return EspMode.Disabled;

				default:
					return EspMode.OnlyPlayers;
			}
		}
	}
}
