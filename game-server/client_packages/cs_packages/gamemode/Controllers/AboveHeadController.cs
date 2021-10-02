// <copyright file="EspController.cs" company="lbyte00">
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
	using RAGE.Elements;

	public class AboveHeadController : Events.Script
	{
		private const int ScreenStaticX = 1920;
		private const int ScreenStaticY = 1080;
		private const float MaxRange = 25 * 25;

		public AboveHeadController()
		{
			Events.Tick += this.OnTick;
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (nametags == null) return;

			int screenX = 0, screenY = 0;
			RAGE.Game.Graphics.GetScreenResolution(ref screenX, ref screenY);

			foreach (Events.TickNametagData nametag in nametags)
			{
				if (nametag.Player == RAGE.Elements.Player.LocalPlayer || (nametag.Player.GetSharedData(DataKey.IsAdmin) != null && (bool)nametag.Player.GetSharedData(DataKey.IsAdmin)))
				{
					continue;
				}

				if (nametag.Distance > MaxRange)
				{
					continue;
				}

				double x = nametag.ScreenX, y = nametag.ScreenY;

				double scale = (nametag.Distance / MaxRange);
				if (scale < 0.6) scale = 0.6;

				y -= scale * (0.005 * (screenY / ScreenStaticY));

				UIResText.Draw(
					$"{nametag.Player.Name} ({nametag.Player.Id})",
					(int)(x * ScreenStaticX),
					(int)(y * ScreenStaticY),
					Font.ChaletLondon, (float)0.4, Color.White, UIResText.Alignment.Centered, false, true, 0);
			}

		}
	}
}
