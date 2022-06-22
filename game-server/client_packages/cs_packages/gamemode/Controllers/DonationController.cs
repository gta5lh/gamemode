// <copyright file="DonationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using GamemodeCommon.Authentication.Models;
	using static GamemodeClient.Controllers.Cef.Cef;
	using Newtonsoft.Json;
	using RAGE;
	using RAGE.Ui;
	using System;
	using GamemodeClient.Models;
	using GamemodeCommon.Models;

	public class DonationController : Events.Script
	{
		public DonationController()
		{
			Events.Add("DisplayDonationNotification", this.OnDisplayDonationNotification);
		}

		private void OnDisplayDonationNotification(object[] request)
		{
			long amount = (long)request[0];
			DisplayNotification(new Notification(string.Format("Тебе на счет было начислено {0} LC. Спасибо за поддержку нашего проекта!", amount), 0, 10000, NotificationType.Success));
			RAGE.Game.Audio.PlaySoundFrontend(-1, "CHECKPOINT_PERFECT", "HUD_MINI_GAME_SOUNDSET", true);
		}
	}
}
