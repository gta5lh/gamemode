namespace GamemodeClient.Controllers
{
	using RAGE.Ui;

	public static class Menu
	{
		public static void Open(bool canInteractWithMenu, bool displayCursor = true, bool hideChat = true)
		{
			if (!canInteractWithMenu)
			{
				return;
			}

			HelpPopUpController.Instance.SetEnabled(false);
			Ui.OpenUI(displayCursor, hideChat);
			RAGE.Game.Ui.DisplayRadar(false);
		}

		public static void Close()
		{
			Ui.CloseUI();
			HelpPopUpController.Instance.SetEnabled(true);
			RAGE.Game.Ui.DisplayRadar(true);
		}
	}
}
