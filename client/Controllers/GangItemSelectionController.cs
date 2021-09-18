using RAGE;
using RAGE.Ui;
using static RAGE.Events;
using static GamemodeClient.Controllers.Cef.Cef;
using GamemodeClient.Models;

namespace GamemodeClient.Controllers
{
	public class GangItemSelectionController : Events.Script
	{
		private bool canInteractWithMenu;
		private bool isInGangItemSelection = false;

		public GangItemSelectionController()
		{
			// Common
			Events.Add("DisplayGangItemSelectionMenu", this.OnDisplayGangItemSelectionMenu);
			Events.Add("CloseGangItemSelectionMenu", this.OnCloseGangItemSelectionMenu);
			Events.Add("PlayerSelectedGangItem", this.OnPlayerSelectedGangItem);

			Events.OnPlayerDeath += this.OnPlayerDeath;
		}

		private void OnDisplayGangItemSelectionMenu(object[] args)
		{
			this.canInteractWithMenu = (bool)args[0];
			HelpPopUpController.Instance.SetEnabled(this.canInteractWithMenu);
			HelpPopUpController.InteractKeyPressed = this.OnInteractKeyPressed;
			HelpPopUpController.ExitKeyPressed = this.OnExitKeyPressed;

			if (!this.canInteractWithMenu)
			{
				HelpPopUpController.InteractKeyPressed = null;
				HelpPopUpController.ExitKeyPressed = null;
				return;
			}
		}

		public void OnInteractKeyPressed()
		{
			if (Cursor.Visible || !this.canInteractWithMenu)
			{
				return;
			}

			ShowWeaponShop(new ShowWeaponShop(Player.Money));
			this.isInGangItemSelection = true;
		}

		private void OnExitKeyPressed()
		{
			this.isInGangItemSelection = false;
			CloseWeaponShop();
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.canInteractWithMenu = false;
			this.isInGangItemSelection = false;
		}

		private void OnCloseGangItemSelectionMenu(object[] args)
		{
			this.OnExitKeyPressed();
		}

		private void OnPlayerSelectedGangItem(object[] args)
		{
			Events.CallRemote("PlayerSelectedGangItem", (string)args[0]);
			DisplayNotification(new Notification("Держи пушку, братушка", 0, 2000, NotificationType.Success));
		}
	}
}
