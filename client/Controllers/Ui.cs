using RAGE;
using RAGE.Ui;

namespace GamemodeClient.Controllers
{
	public class Ui
	{
		public static void OpenUI(HtmlWindow window, bool displayCursor = true, bool hideChat = true)
		{
			Chat.Show(!hideChat);
			Player.CurrentPlayer.FreezePosition(true);
			Cursor.Visible = displayCursor;
		}

		public static void CloseUI(HtmlWindow window)
		{
			Cursor.Visible = false;
			Chat.Show(true);
			Player.CurrentPlayer.FreezePosition(false);
		}
	}
}
