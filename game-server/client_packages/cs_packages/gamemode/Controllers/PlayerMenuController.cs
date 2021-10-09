// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using static GamemodeClient.Controllers.Cef.Cef;
	using RAGE;
	using RAGE.Ui;
	using GamemodeClient.Models;

	public partial class PlayerMenuController : Events.Script
	{
		private bool playerMenuEnabled = false;

		public PlayerMenuController()
		{
			Events.Add("ClosePlayerMenu", this.OnClosePlayerMenu);

			RAGE.Input.Bind(VirtualKeys.M, false, this.OnDisplayPlayerMenu);
			RAGE.Input.Bind(VirtualKeys.Escape, false, this.OnExitKeyPressed);
		}

		private void OnDisplayPlayerMenu()
		{
			this.displayPlayerMenu(!playerMenuEnabled);
		}

		private void OnExitKeyPressed()
		{
			this.displayPlayerMenu(false);
		}

		private void displayPlayerMenu(bool display)
		{
			if (Cursor.Visible && (!this.playerMenuEnabled))
			{
				return;
			}

			this.playerMenuEnabled = display;
			Cursor.Visible = this.playerMenuEnabled;
			if (this.playerMenuEnabled)
			{
				string playerName = Player.CurrentPlayer.Name;
				if (playerName.Length > 9)
				{
					playerName = playerName.Substring(9);
				}

				playerName = $"{playerName} [{Player.CurrentPlayer.Id}]";
				ShowPlayerMenu(new ShowPlayerMenu(playerName, Player.Money, Player.FractionRankName, Player.FractionName, Player.CurrentExperience.ToString(), "-"));
			}
			else
			{
				HidePlayerMenu();
			}
		}

		public void OnClosePlayerMenu(object[] args)
		{
			this.OnExitKeyPressed();
		}
	}
}
