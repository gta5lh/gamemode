// <copyright file="Spectate.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System;
	using System.Collections.Generic;
	using GamemodeClient.Game;
	using RAGE;
	using RAGE.Elements;
	using Cam = RAGE.Game.Cam;

	public class Spectate : Events.Script
	{
		private const short UpdateTimeIntervalMilliseconds = 500;

		private const float Sensitivity = 0.15f;

		private static int camera;
		private static int specId = -1;
		private static Player? specPlayer;

		private static float angleY;
		private static float angleZ;
		private static float radius = 6.0f;

		private static DateTime nextUpdateTime = DateTime.MinValue;

		static Spectate()
		{
			Events.Tick += OnTick;
			Events.Add("spectate", Spec);
			Events.Add("spectateStop", DisableSpec);
		}

		public static void Spec(object[] args)
		{
			specId = (int)args[0];
			Game.Player.Models.Player.Spectating = true;
			Game.Player.Models.Player.CurrentPlayer.FreezePosition(true);
		}

		private static void OnTick(List<Events.TickNametagData> nametags)
		{
			if (specPlayer == null && Game.Player.Models.Player.Spectating)
			{
				if (DateTime.UtcNow < nextUpdateTime)
				{
					return;
				}

				foreach (Player player in Entities.Players.Streamed)
				{
					if (player.RemoteId != specId)
					{
						continue;
					}

					EnableSpec(player);
					return;
				}

				nextUpdateTime = DateTime.UtcNow.AddMilliseconds(UpdateTimeIntervalMilliseconds);
				return;
			}

			if (specPlayer == null)
			{
				return;
			}

			RAGE.Game.Pad.DisableControlAction(2, 16, true);

			if (!specPlayer.Exists)
			{
				Events.CallRemote("TrySpectate", specPlayer.RemoteId);
				specPlayer = null!;
				return;
			}

			UpdateCameraPosition();
		}

		private static void UpdateCameraPosition()
		{
			Player.LocalPlayer.Position = specPlayer!.Position + new Vector3(0, 0, 2);

			Vector3 newPos = GetPos();

			Cam.SetCamCoord(camera, newPos.X, newPos.Y, newPos.Z);
			Cam.PointCamAtCoord(camera, specPlayer.Position.X, specPlayer.Position.Y, specPlayer.Position.Z + 0.5f);
		}

		private static void EnableSpec(Player player)
		{
			specPlayer = player;
			specId = -1;

			if (!Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetInvincible(true);
				Game.Player.Models.Player.CurrentPlayer.SetAlpha(0, false);
				Game.Player.Models.Player.CurrentPlayer.SetVisible(false, false);
				Game.Player.Models.Player.CurrentPlayer.SetCollision(false, false);
			}

			if (Cam.DoesCamExist(camera))
			{
				Cam.RenderScriptCams(false, false, 0, true, false, 0);
				Cam.DestroyCam(camera, false);
			}

			camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey(Constants.CameraName), specPlayer.Position.X, specPlayer.Position.Y, specPlayer.Position.Z, 0, 0, 0, Cam.GetGameplayCamFov(), true, 2);
			Cam.SetCamActive(camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);
			UpdateCameraPosition();

			Game.Player.Models.Player.CurrentPlayer.SetCurrentWeaponVisible(false, true, true, true);
			Game.Player.Models.Player.Spectating = true;
		}

		private static void DisableSpec(object[] args)
		{
			if (!Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetCurrentWeaponVisible(true, false, true, true);
				Game.Player.Models.Player.CurrentPlayer.SetAlpha(255, false);
				Game.Player.Models.Player.CurrentPlayer.SetVisible(true, false);
			}

			Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Cam.DestroyCam(camera, false);
			Game.Player.Models.Player.CurrentPlayer.FreezePosition(false);
			Game.Player.Models.Player.CurrentPlayer.SetCollision(true, false);

			if (!Game.Player.Models.Player.GodmodEnabled && !Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetInvincible(false);
			}

			specPlayer = null;
			Game.Player.Models.Player.Spectating = false;
		}

		private static Vector3 GetPos()
		{
			angleZ -= RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * Sensitivity; //-- around Z axis (left / right)
			angleY += RAGE.Game.Pad.GetDisabledControlNormal(1, 2) * Sensitivity; //-- up / down
			radius += (RAGE.Game.Pad.GetDisabledControlNormal(1, 14) + (RAGE.Game.Pad.GetDisabledControlNormal(1, 15) * -1)) * Sensitivity; //-- zoom

			//-- limit up / down angle to 90°
			if (angleY > 1.5f)
			{
				angleY = 1.5f;
			}
			else if (angleY < -1.5f)
			{
				angleY = -1.5f;
			}

			Vector3 pCoords = specPlayer!.Position;

			Vector3 behindCam = new Vector3(
				(float)(pCoords.X + (((Math.Cos(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Cos(angleZ))) / 2 * (radius + 0.5f))),
				(float)(pCoords.Y + (((Math.Sin(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Sin(angleZ))) / 2 * (radius + 0.5f))),
				(float)(pCoords.Z + (Math.Sin(angleY) * (radius + 0.5f))));

			int rayHandle = RAGE.Game.Shapetest.StartShapeTestRay(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, behindCam.X, behindCam.Y, behindCam.Z, -1, specPlayer.Handle, 0);

			int hitBool = 0;
			Vector3 hitCoords = new Vector3();
			Vector3 surfaceNormal = new Vector3();
			int entityHit = 0;

			RAGE.Game.Shapetest.GetShapeTestResult(rayHandle, ref hitBool, hitCoords, surfaceNormal, ref entityHit);

			float maxRadius = radius;
			if (Convert.ToBoolean(hitBool) && Vdist(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, hitCoords) < radius + 0.5f)
			{
				maxRadius = Vdist(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, hitCoords);
			}

			Vector3 offset = new Vector3(
				(float)(((Math.Cos(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Cos(angleZ))) / 2 * maxRadius),
				(float)(((Math.Sin(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Sin(angleZ))) / 2 * maxRadius),
				(float)(Math.Sin(angleY) * maxRadius));

			return new Vector3(pCoords.X + offset.X, pCoords.Y + offset.Y, pCoords.Z + offset.Z);
		}

		private static float Vdist(float x, float y, float z, Vector3 v2)
		{
			return v2.DistanceTo(new Vector3(x, y, z));
		}
	}
}
