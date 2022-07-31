// <copyright file="Esp.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using System.Drawing;
	using GamemodeClient.Game.Vehicle;
	using GamemodeCommon.Models.Data;
	using RAGE;
	using RAGE.Game;
	using RAGE.NUI;
	using RAGE.Ui;

	public class Esp : Events.Script
	{
		private const int ScreenStaticX = 1920;
		private const int ScreenStaticY = 1080;
		private const float AddForAboveHead = 0.6f;
		private const float MaxDistanceVehicle = 50;
		private const float MaxDistancePlayer = 200;
		private const float VehicleScale = 0.3f;
		private const float PlayerScale = 0.25f;

		private static EspMode espMode;

		static Esp()
		{
			Input.Bind(VirtualKeys.F12, false, OnEspKeyPressed);
			Events.Tick += OnTick;
		}

		private static void OnEspKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			if (GamemodeClient.Game.Player.Models.Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin) == null || !(bool)GamemodeClient.Game.Player.Models.Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin))
			{
				return;
			}

			espMode = espMode.IncreaseEspMode();
			Chat.Output("Вы сменили ESP режим на " + espMode.DisplayMode());
		}

		private static void OnTick(List<Events.TickNametagData> nametags)
		{
			if (espMode == EspMode.Disabled)
			{
				return;
			}

			if (espMode == EspMode.OnlyPlayers || espMode == EspMode.PlayersWithCars)
			{
				RAGE.Elements.Entities.Players.Streamed.ForEach((player) =>
				{
					if (GamemodeClient.Game.Player.Models.Player.CurrentPlayer.Id == player.Id || GamemodeClient.Game.Player.Models.Player.CurrentPlayer.Position.DistanceTo(player.Position) > MaxDistancePlayer)
					{
						return;
					}

					float screenX = 0;
					float screenY = 0;

					if (Graphics.GetScreenCoordFromWorldCoord(player.Position.X, player.Position.Y, player.Position.Z, ref screenX, ref screenY))
					{
						UIResText.Draw($"[SID: {player.GetSharedData(DataKey.StaticId)}], [DID: {player.Id}], [NAME: {player.Name}]~n~[HP: {player.GetHealth()}/{player.GetMaxHealth()}], [SPEED: {Speed.GetPlayerRealSpeed(player)}]", (int)(screenX * ScreenStaticX), (int)(screenY * ScreenStaticY), Font.ChaletLondon, PlayerScale, Color.White, UIResText.Alignment.Centered, false, true, 0);
					}
				});
			}

			if (espMode == EspMode.PlayersWithCars)
			{
				RAGE.Elements.Entities.Vehicles.Streamed.ForEach((vehicle) =>
				{
					if (GamemodeClient.Game.Player.Models.Player.CurrentPlayer.Position.DistanceTo(vehicle.Position) > MaxDistanceVehicle)
					{
						return;
					}

					float screenX = 0;
					float screenY = 0;

					if (Graphics.GetScreenCoordFromWorldCoord(vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z + AddForAboveHead, ref screenX, ref screenY))
					{
						UIResText.Draw($"[ID: {vehicle.Id}], [NAME: {Vehicle.GetDisplayNameFromVehicleModel(vehicle.Model).ToLower()}]~n~[HP: {vehicle.GetHealth()}/{vehicle.GetMaxHealth()}]", (int)(screenX * ScreenStaticX), (int)(screenY * ScreenStaticY), Font.ChaletLondon, VehicleScale, Color.White, UIResText.Alignment.Centered, false, true, 0);
					}
				});
			}
		}
	}
}
