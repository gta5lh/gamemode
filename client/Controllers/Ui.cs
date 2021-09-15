// <copyright file="Ui.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Ui;

	public class Ui
	{
		public static void OpenUI(bool displayCursor = true, bool hideChat = true)
		{
			Chat.Show(!hideChat);
			Player.CurrentPlayer.FreezePosition(true);
			Cursor.Visible = displayCursor;
		}

		public static void CloseUI()
		{
			Cursor.Visible = false;
			Chat.Show(true);
			Player.CurrentPlayer.FreezePosition(false);
		}
	}
}
