using RAGE;
using RAGE.Ui;

namespace GamemodeClient.Controllers
{
	public class DisableUIController : Events.Script
	{
		private bool uiEnabled = true;

		public DisableUIController()
		{
			RAGE.Input.Bind(VirtualKeys.Numpad0, false, this.OnDisableUI);
		}

		private void OnDisableUI()
		{
			if (Cursor.Visible) return;

			uiEnabled = !uiEnabled;

			Chat.Show(uiEnabled);
			RAGE.Game.Ui.DisplayRadar(uiEnabled);
			if (uiEnabled)
			{
				ScreenController.DisplayTopRightMenu();
			}
			else
			{
				ScreenController.DisableTopRightMenu();
			}

			string enabledString = uiEnabled ? "включен" : "выключен";
			Chat.Output($"UI был {enabledString}");
		}
	}
}
