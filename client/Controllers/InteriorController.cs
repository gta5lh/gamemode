// <copyright file="InteriorController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using System.Drawing;
	using RAGE;
	using RAGE.Game;
	using RAGE.NUI;
	using RAGE.Ui;
	using GamemodeCommon.Models.Data;
	using GamemodeClient.Services;

	public class InteriorController : Events.Script
	{
		public InteriorController()
		{
			Events.Add("TeleportToInterior", this.OnTeleportToInterior);
		}

		public async void OnTeleportToInterior(object[] args)
		{
			RAGE.Game.Streaming.RequestIpl((string)args[0]);
			Player.CurrentPlayer.Position = new Vector3((float)args[1], (float)args[2], (float)args[3]);
		}
	}
}
