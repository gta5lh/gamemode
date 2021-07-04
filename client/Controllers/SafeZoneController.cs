using RAGE;
using System.Collections.Generic;

namespace GamemodeClient.Controllers
{
	public class SafeZoneController : Events.Script
	{
		private bool IsInSafeZone = false;

		public SafeZoneController()
		{
			Events.Tick += OnTick;
			Events.Add("safeZone", OnSafeZone);
		}

		public void OnTick(List<Events.TickNametagData> nametags)
		{
			if (!IsInSafeZone) return;

			RAGE.Game.Player.DisablePlayerFiring(true);
		}

		public void OnSafeZone(object[] args)
		{
			bool enabled = (bool)args[0];

			IsInSafeZone = enabled;

			string message = IsInSafeZone ? "вошел в зеленую зону" : "вышел из зеленой зоны";
			Chat.Output($"Ты {message}");

			if (!Player.NoclipEnabled && !Player.GodmodEnabled && !Player.InvisibilityEnabled)
			{
				RAGE.Elements.Player.LocalPlayer.SetCanBeDamaged(!enabled);
				Player.CurrentPlayer.SetInvincible(enabled);
			}
		}
	}
}
