// <copyright file="Player.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.Player.Models
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

		public delegate void MoneyUpdatedDelegate(long money);

		public static event MoneyUpdatedDelegate MoneyUpdatedEvent;

		public delegate void ExperienceUpdatedDelegate(long previousExperience, long currentExperience, long requiredExperience);

		public static event ExperienceUpdatedDelegate ExperienceUpdatedEvent;

		public static long Money { get; set; } = 0;

		public static long CurrentExperience { get; set; } = 0;

		public static long PreviousExperience { get; set; } = 0;

		public static long RequiredExperience { get; set; } = 0;

		public static string FractionRankName { get; set; } = "-";

		public static string FractionName { get; set; } = "-";

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
			Events.Add("ExperienceUpdated", this.OnExperienceUpdated);
			Events.Add("FractionRankNameUpdated", this.OnFractionRankNameUpdated);
			Events.Add("FractionNameUpdated", this.OnFractionNameUpdated);
		}

		public static bool IsInVehicle()
		{
			return Player.CurrentPlayer.Vehicle?.Exists == true;
		}

		private void OnMoneyUpdated(object[] request)
		{
			Money = (long)request[0];
			MoneyUpdatedEvent(Money);
		}

		private void OnExperienceUpdated(object[] args)
		{
			PreviousExperience = (long)args[0];
			CurrentExperience = (long)args[1];

			if (args[2] != null)
			{
				RequiredExperience = (long)args[2];
			}

			ExperienceUpdatedEvent(PreviousExperience, CurrentExperience, RequiredExperience);
		}

		private void OnFractionRankNameUpdated(object[] args)
		{
			if (args == null || args.Length < 1 || ((string)args[0])?.Length == 0 || args[0] == null)
			{
				FractionRankName = "-";
				return;
			}

			FractionRankName = (string)args[0];
		}

		private void OnFractionNameUpdated(object[] args)
		{
			if (args == null || args.Length < 1 || ((string)args[0])?.Length == 0 || args[0] == null)
			{
				FractionName = "-";
				return;
			}

			FractionName = (string)args[0];
		}
	}
}
