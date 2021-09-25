using RAGE;
using RAGE.Ui;
using static RAGE.Events;
using static GamemodeClient.Controllers.Cef.Cef;
using GamemodeClient.Models;
using GamemodeCommon.Models;
using RAGE.Elements;
using Newtonsoft.Json;

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
			Events.OnPlayerEnterVehicle += this.OnPlayerEnterVehicle;
			Player.moneyUpdatedEvent += this.OnMoneyUpdated;
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
			if (!this.canInteractWithMenu)
			{
				return;
			}

			this.isInGangItemSelection = false;
			this.OnDisplayGangItemSelectionMenu(new object[] { false });
			CloseWeaponShop();
		}

		private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
		{
			if (!this.canInteractWithMenu)
			{
				return;
			}

			this.OnDisplayGangItemSelectionMenu(new object[] { false });
		}

		private void OnCloseGangItemSelectionMenu(object[] args)
		{
			this.OnExitKeyPressed();
		}

		private async void OnPlayerSelectedGangItem(object[] args)
		{
			string result = (string)await Events.CallRemoteProc("PlayerSelectedGangItem", (string)args[0], (bool)args[1]);
			GangItemResponse gangItemResponse = JsonConvert.DeserializeObject<GangItemResponse>(result);
			DisplayNotification(new Notification(gangItemResponse.Text, 0, 2000, gangItemResponse.NotificationType));
		}

		private void OnMoneyUpdated(long money)
		{
			UpdateWeaponShopBalance(money);
		}
	}
}
