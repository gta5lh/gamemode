// <copyright file="Player.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Game;

	public class Player : Events.Script
	{
		public const string Stamina = "SP0_STAMINA";
		public const string Shooting = "SP0_SHOOTING_ABILITY";
		public const string Strength = "SP0_STRENGTH";
		public const string Stealth = "SP0_STEALTH_ABILITY";
		public const string Flying = "SP0_FLYING_ABILITY";
		public const string Wheelie = "SP0_WHEELIE_ABILITY";
		public const string Lung = "SP0_LUNG_CAPACITY";

		public delegate void moneyUpdatedDelegate(long money);
		public static event moneyUpdatedDelegate moneyUpdatedEvent;

		public static long Money { get; set; } = 0;
		public static RAGE.Elements.Player CurrentPlayer = RAGE.Elements.Player.LocalPlayer;
		public static bool GodmodEnabled = false;
		public static bool InvisibilityEnabled = false;
		public static bool NoclipEnabled = false;
		public static bool Spectating = false;
		public const int MaxHealth = 200;
		public static bool AuthenticationScreen = true;

		public Player()
		{
			Stats.StatSetInt(Misc.GetHashKey(Flying), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Lung), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Shooting), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Stamina), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Stealth), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Strength), 100, false);
			Stats.StatSetInt(Misc.GetHashKey(Wheelie), 100, false);

			Events.Add("MoneyUpdated", this.OnMoneyUpdated);
		}

		public static bool IsInVehicle()
		{
			return Player.CurrentPlayer.Vehicle != null && Player.CurrentPlayer.Vehicle.Exists;
		}

		private void OnMoneyUpdated(object[] request)
		{
			Money = (long)request[0];
			moneyUpdatedEvent(Money);
		}
	}
}
