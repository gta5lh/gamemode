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

	public class EspController : Events.Script
	{
		private const int ScreenStaticX = 1920;
		private const int ScreenStaticY = 1080;
		private const float AddForAboveHead = 0.6f;
		private const float MaxDistanceVehicle = 50;
		private const float MaxDistancePlayer = 200;
		private const float VehicleScale = 0.3f;
		private const float PlayerScale = 0.25f;
		private const Font BaseFont = Font.ChaletLondon;

		private EspMode espMode;

		public EspController()
		{
			RAGE.Input.Bind(VirtualKeys.F12, false, this.OnEspKeyPressed);
			Events.Tick += this.OnTick;
		}

		private void OnEspKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			if (Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin) == null || !(bool)Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin))
			{
				return;
			}

			this.espMode = this.espMode.IncreaseEspMode();
			Chat.Output("Вы сменили ESP режим на " + this.espMode.DisplayMode());
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (this.espMode == EspMode.Disabled)
			{
				return;
			}

			if (this.espMode == EspMode.OnlyPlayers || this.espMode == EspMode.PlayersWithCars)
			{
				RAGE.Elements.Entities.Players.Streamed.ForEach((player) =>
				{
					if (Player.CurrentPlayer.Id == player.Id || Player.CurrentPlayer.Position.DistanceTo(player.Position) > MaxDistancePlayer)
					{
						return;
					}

					float screenX = 0;
					float screenY = 0;

					if (Graphics.GetScreenCoordFromWorldCoord(player.Position.X, player.Position.Y, player.Position.Z, ref screenX, ref screenY))
					{
						UIResText.Draw(
							$"[SID: {player.GetSharedData(DataKey.StaticId)}], [DID: {player.Id}], [NAME: {player.Name}]~n~[HP: {player.GetHealth()}/{player.GetMaxHealth()}], [SPEED: {Speed.GetPlayerRealSpeed(player)}]",
							(int)(screenX * ScreenStaticX),
							(int)(screenY * ScreenStaticY),
							Font.ChaletLondon, PlayerScale, Color.White, UIResText.Alignment.Centered, false, true, 0);
					}
				});
			}

			if (this.espMode == EspMode.PlayersWithCars)
			{
				RAGE.Elements.Entities.Vehicles.Streamed.ForEach((vehicle) =>
				{
					if (Player.CurrentPlayer.Position.DistanceTo(vehicle.Position) > MaxDistanceVehicle)
					{
						return;
					}

					float screenX = 0;
					float screenY = 0;

					if (Graphics.GetScreenCoordFromWorldCoord(vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z + AddForAboveHead, ref screenX, ref screenY))
					{
						UIResText.Draw(
							$"[ID: {vehicle.Id}], [NAME: {Vehicle.GetDisplayNameFromVehicleModel(vehicle.Model).ToLower()}]~n~[HP: {vehicle.GetHealth()}/{vehicle.GetMaxHealth()}]",
							(int)(screenX * ScreenStaticX),
							(int)(screenY * ScreenStaticY),
							Font.ChaletLondon, VehicleScale, Color.White, UIResText.Alignment.Centered, false, true, 0);
					}
				});
			}
		}
	}

	public enum EspMode
	{
		Disabled,
		OnlyPlayers,
		PlayersWithCars
	}

	public static class EspModeMethods
	{
		public static EspMode IncreaseEspMode(this EspMode espMode)
		{
			switch (espMode)
			{
				case EspMode.OnlyPlayers:
					return EspMode.PlayersWithCars;

				case EspMode.PlayersWithCars:
					return EspMode.Disabled;

				default:
					return EspMode.OnlyPlayers;
			}
		}

		public static string DisplayMode(this EspMode espMode)
		{
			switch (espMode)
			{
				case EspMode.OnlyPlayers:
					return "только игроки";

				case EspMode.PlayersWithCars:
					return "игроки с машинами";

				default:
					return "выключен";
			}
		}
	}
}
