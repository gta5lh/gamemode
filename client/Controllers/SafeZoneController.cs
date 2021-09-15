using System.Collections.Generic;
using RAGE;

namespace GamemodeClient.Controllers
{
	public class SafeZoneController : Events.Script
	{
		public delegate void playerSafeZoneDelegate(bool enabled);
		public static event playerSafeZoneDelegate playerSafeZoneEvent;

		private bool IsInSafeZone = false;

		public SafeZoneController()
		{
			Events.Tick += this.OnTick;
			Events.Add("safeZone", this.OnSafeZone);
		}

		public void OnTick(List<Events.TickNametagData> nametags)
		{
			if (!IsInSafeZone)
			{
				return;
			}

			RAGE.Game.Player.DisablePlayerFiring(true);
		}

		public void OnSafeZone(object[] args)
		{
			bool enabled = (bool)args[0];

			this.IsInSafeZone = enabled;
			playerSafeZoneEvent(this.IsInSafeZone);

			if (!Player.NoclipEnabled && !Player.GodmodEnabled && !Player.InvisibilityEnabled)
			{
				RAGE.Elements.Player.LocalPlayer.SetCanBeDamaged(!enabled);
				Player.CurrentPlayer.SetInvincible(enabled);
			}
		}
	}
}
