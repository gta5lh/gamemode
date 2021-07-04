namespace GamemodeClient.Controllers
{
	using RAGE.Ui;

	public static class Menu
	{
		public static HtmlWindow Open(bool canInteractWithMenu, HtmlWindow menu, string menuPath, bool displayCursor = true, bool hideChat = true)
		{
			if (!canInteractWithMenu)
			{
				return null;
			}

			if (menu != null)
			{
				return menu;
			}

			HelpPopUpController.Instance.SetEnabled(false);
			menu = new HtmlWindow(menuPath);
			Ui.OpenUI(menu, displayCursor, hideChat);
			RAGE.Game.Ui.DisplayRadar(false);

			return menu;
		}

		public static bool Close(ref HtmlWindow? menu)
		{
			if (menu == null)
			{
				return false;
			}

			Ui.CloseUI(menu);
			RAGE.Game.Ui.DisplayRadar(true);
			menu.Destroy();
			HelpPopUpController.Instance.SetEnabled(true);
			menu = null;

			return true;
		}
	}
}
