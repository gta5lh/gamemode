using RAGE;
using RAGE.Ui;

namespace GamemodeClient.Controllers
{
	public class DisableUIController : Events.Script
	{
		public delegate void disableUIStateChangedDelegate(bool enabled);
		public static event disableUIStateChangedDelegate disableUIStateChangedEvent;

		private bool uiEnabled = true;

		public DisableUIController()
		{
			RAGE.Input.Bind(VirtualKeys.F5, false, this.OnDisableUI);
		}

		private void OnDisableUI()
		{
			if (Cursor.Visible) return;

			uiEnabled = !uiEnabled;

			Chat.Show(uiEnabled);
			RAGE.Game.Ui.DisplayRadar(uiEnabled);

			disableUIStateChangedEvent(uiEnabled);
		}
	}
}
