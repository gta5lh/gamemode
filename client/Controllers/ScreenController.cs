using System;
using System.Collections.Generic;
using RAGE;
using RAGE.Ui;

namespace GamemodeClient.Controllers
{
	public class ScreenController : Events.Script
	{
		private static string TopRightMenuPath = $"package://cs_packages/gamemode/Frontend/Screen/TopRight/index.html";
		private static HtmlWindow TopRightMenu;
		private static bool Enabled = false;

		public ScreenController()
		{
			// Common
			Events.Add("DisplayTopRightMenu", OnDisplayTopRightMenu);
			Events.Add("CloseTopRightMenu", OnCloseTopRightMenu);

			Events.Tick += this.Tick;

			TopRightMenu = new HtmlWindow(TopRightMenuPath);
			TopRightMenu.Active = false;
		}

		public static void DisplayTopRightMenu()
		{
			TopRightMenu.Active = true;
			UpdateOnline();
			Enabled = true;
		}

		public static void DisableTopRightMenu()
		{
			TopRightMenu.Active = false;
			Enabled = false;
		}

		private static void OnDisplayTopRightMenu(object[] args)
		{
			DisplayTopRightMenu();
		}

		private static void OnCloseTopRightMenu(object[] args)
		{
			DisableTopRightMenu();
		}

		private DateTime NextOnlineUpdateTime = DateTime.MinValue;
		private const byte UpdateOnlineTimeIntervalSeconds = 5;
		private const int HudWeapon = 2;
		private const int HudCash = 3;
		private const int WeaponWheelStats = 20;

		private void Tick(List<Events.TickNametagData> nametags)
		{
			if (!Enabled) return;

			this.HideHudComponentIfActive(HudWeapon);
			this.HideHudComponentIfActive(HudCash);

			if (TopRightMenu.Active && (RAGE.Game.Ui.IsHudComponentActive(WeaponWheelStats) || RAGE.Game.Ui.IsPauseMenuActive()))
			{
				TopRightMenu.Active = false;
			}
			else if (!TopRightMenu.Active && (!RAGE.Game.Ui.IsHudComponentActive(WeaponWheelStats) && !RAGE.Game.Ui.IsPauseMenuActive()))
			{
				TopRightMenu.Active = true;
			}

			if (!TopRightMenu.Active || DateTime.UtcNow < this.NextOnlineUpdateTime)
			{
				return;
			}

			UpdateOnline();
			this.NextOnlineUpdateTime = DateTime.UtcNow.AddSeconds(UpdateOnlineTimeIntervalSeconds);
		}

		private static void UpdateOnline()
		{
			TopRightMenu.ExecuteJs($"setOnline({RAGE.Elements.Entities.Players.Count})");
		}

		private void HideHudComponentIfActive(int hud)
		{
			if (RAGE.Game.Ui.IsHudComponentActive(hud))
			{
				RAGE.Game.Ui.HideHudComponentThisFrame(hud);
			}
		}
	}
}
