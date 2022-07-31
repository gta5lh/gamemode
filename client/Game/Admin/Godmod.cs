// <copyright file="Godmod.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.Admin
{
	using GamemodeClient.Game.Player.Models;
	using RAGE;

	public class Godmod : Events.Script
	{
		static Godmod()
		{
			Events.Add("SetGodmod", OnSetGodmod);
			Events.Add("SetInvisibility", OnSetInvisibility);
		}

		private static void OnSetGodmod(object[] args)
		{
			Player.GodmodEnabled = !Player.GodmodEnabled;
			if (!Player.InvisibilityEnabled)
			{
				RAGE.Elements.Player.LocalPlayer.SetCanBeDamaged(!Player.GodmodEnabled);
				RAGE.Elements.Player.LocalPlayer.SetInvincible(Player.GodmodEnabled);
				Player.CurrentPlayer.SetCanRagdoll(!Player.GodmodEnabled);
			}

			string enabledString = Player.GodmodEnabled ? "включили" : "выключили";
			Chat.Output($"Вы {enabledString} себе годмод");
		}

		private static void OnSetInvisibility(object[] args)
		{
			Player.InvisibilityEnabled = !Player.InvisibilityEnabled;
			if (!Player.GodmodEnabled)
			{
				RAGE.Elements.Player.LocalPlayer.SetCanBeDamaged(!Player.InvisibilityEnabled);
				RAGE.Elements.Player.LocalPlayer.SetInvincible(Player.InvisibilityEnabled);
				Player.CurrentPlayer.SetCanRagdoll(!Player.InvisibilityEnabled);
			}

			Player.CurrentPlayer.SetAlpha(Player.InvisibilityEnabled ? 0 : 255, false);
			Player.CurrentPlayer.SetVisible(!Player.InvisibilityEnabled, false);
			Player.CurrentPlayer.SetCurrentWeaponVisible(!Player.InvisibilityEnabled, Player.InvisibilityEnabled, true, true);

			string enabledString = Player.InvisibilityEnabled ? "включили" : "выключили";
			Chat.Output($"Вы {enabledString} себе невидимку");
		}
	}
}
