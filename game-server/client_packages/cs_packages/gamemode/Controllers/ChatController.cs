// <copyright file="ExperienceController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeClient.Models;
	using Newtonsoft.Json;
	using RAGE;
	using RAGE.Ui;
	using static GamemodeClient.Controllers.Cef.Cef;

	public partial class HudController : Events.Script
	{
		private bool ChatOpened = true;

		private void OnOpenChatKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			this.ChatOpened = !this.ChatOpened;
			if (this.ChatOpened)
			{
				OpenChat();
				Cursor.Visible = true;
			}
			else
			{
				CloseChat();
				Cursor.Visible = false;
			}
		}

		private void OnSendChatMessage(object[] request)
		{
			CloseChat();
			Cursor.Visible = false;
		}

		private void OnCloseChat(object[] request)
		{
			CloseChat();
			Cursor.Visible = false;
		}
	}
}
