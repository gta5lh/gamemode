// <copyright file="Player.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;

	public class Player : Events.Script
	{
		public static RAGE.Elements.Player CurrentPlayer = RAGE.Elements.Player.LocalPlayer;
		public static bool GodmodEnabled = false;
		public static bool InvisibilityEnabled = false;
		public static bool NoclipEnabled = false;
		public static bool Spectating = false;
		public const int MaxHealth = 200;
		public static bool AuthenticationScreen = true;

		public static bool IsInVehicle()
		{
			return Player.CurrentPlayer.Vehicle != null && Player.CurrentPlayer.Vehicle.Exists;
		}
	}
}
