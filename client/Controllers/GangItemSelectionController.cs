using RAGE;
using RAGE.Ui;
using static RAGE.Events;

namespace GamemodeClient.Controllers
{
	public class GangItemSelectionController : Events.Script
	{
		private bool canInteractWithMenu;

		private string MenuPath = $"package://cs_packages/gamemode/Frontend/Gang/Item/index.html";
		private HtmlWindow? Menu;

		public GangItemSelectionController()
		{
			// Common
			Events.Add("DisplayGangItemSelectionMenu", this.OnDisplayGangItemSelectionMenu);
			Events.Add("CloseGangItemSelectionMenu", this.OnCloseGangItemSelectionMenu);
			Events.Add("PlayerSelectedGangItem", this.OnPlayerSelectedGangItem);

			Events.OnPlayerDeath += OnPlayerDeath;
		}

		private void OnDisplayGangItemSelectionMenu(object[] args)
		{
			this.canInteractWithMenu = (bool)args[0];
			HelpPopUpController.Instance.SetEnabled(this.canInteractWithMenu);
			HelpPopUpController.InteractKeyPressed = this.OnInteractKeyPressed;
			HelpPopUpController.ExitKeyPressed = this.OnExitKeyPressed;
		}

		public void OnInteractKeyPressed()
		{
			if (Cursor.Visible) return;

			this.Menu = Controllers.Menu.Open(this.canInteractWithMenu, this.Menu, this.MenuPath);
		}

		private void OnExitKeyPressed()
		{
			Controllers.Menu.Close(ref this.Menu);
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.canInteractWithMenu = false;
			Controllers.Menu.Close(ref this.Menu);
		}

		private void OnCloseGangItemSelectionMenu(object[] args)
		{
			Controllers.Menu.Close(ref this.Menu);
		}

		private void OnPlayerSelectedGangItem(object[] args)
		{
			Events.CallRemote("PlayerSelectedGangItem", (string)args[0]);
		}
	}
}
