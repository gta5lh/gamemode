using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using Cam = RAGE.Game.Cam;

namespace GamemodeClient.Controllers
{
	public class SpectateController : Events.Script
	{
		private float Sensitivity = 0.15f;

		private int Camera;
		private int SpecId = -1;
		private RAGE.Elements.Player SpecPlayer = null;

		private float angleY = 0.0f;
		private float angleZ = 0.0f;
		private float radius = 6.0f;

		private DateTime NextUpdateTime = DateTime.MinValue;

		private const short UpdateTimeIntervalMilliseconds = 500;

		public SpectateController()
		{
			Events.Tick += OnTick;
			Events.Add("spectate", Spec);
			Events.Add("spectateStop", DisableSpec);
		}

		public void Spec(object[] args)
		{
			SpecId = (int)args[0];
			Player.Spectating = true;
			Player.CurrentPlayer.FreezePosition(true);
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (SpecPlayer == null && Player.Spectating)
			{
				if (DateTime.UtcNow < this.NextUpdateTime)
				{
					return;
				}

				foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
				{
					if (player.RemoteId != SpecId) continue;
					EnableSpec(player);
					return;
				}

				this.NextUpdateTime = DateTime.UtcNow.AddMilliseconds(UpdateTimeIntervalMilliseconds);
				return;
			}

			if (SpecPlayer == null) return;

			RAGE.Game.Pad.DisableControlAction(2, 16, true);

			if (!SpecPlayer.Exists)
			{
				Events.CallRemote("TrySpectate", SpecPlayer.RemoteId);
				SpecPlayer = null;
				return;
			}

			UpdateCameraPosition();
		}

		private void UpdateCameraPosition()
		{
			RAGE.Elements.Player.LocalPlayer.Position = SpecPlayer.Position + new Vector3(0, 0, 10);

			Vector3 newPos = GetPos();

			Cam.SetCamCoord(Camera, newPos.X, newPos.Y, newPos.Z);
			Cam.PointCamAtCoord(Camera, SpecPlayer.Position.X, SpecPlayer.Position.Y, SpecPlayer.Position.Z + 0.5f);
		}

		private void EnableSpec(RAGE.Elements.Player player)
		{
			SpecPlayer = player;
			SpecId = -1;

			if (!Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(true);
				Player.CurrentPlayer.SetAlpha(0, false);
				Player.CurrentPlayer.SetVisible(false, false);
				Player.CurrentPlayer.SetCollision(false, false);
			}

			if (Cam.DoesCamExist(Camera))
			{
				Cam.RenderScriptCams(false, false, 0, true, false, 0);
				Cam.DestroyCam(Camera, false);
			}

			Camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey(Constants.CameraName), SpecPlayer.Position.X, SpecPlayer.Position.Y, SpecPlayer.Position.Z, 0, 0, 0, Cam.GetGameplayCamFov(), true, 2);
			Cam.SetCamActive(Camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);
			UpdateCameraPosition();

			Player.CurrentPlayer.SetCurrentWeaponVisible(false, true, true, true);
			Player.Spectating = true;
		}

		private void DisableSpec(object[] args)
		{
			if (!Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetCurrentWeaponVisible(true, false, true, true);
				Player.CurrentPlayer.SetAlpha(255, false);
				Player.CurrentPlayer.SetVisible(true, false);
			}

			Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Cam.DestroyCam(Camera, false);
			Player.CurrentPlayer.FreezePosition(false);
			Player.CurrentPlayer.SetCollision(true, false);

			if (!Player.GodmodEnabled && !Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(false);
			}

			SpecPlayer = null;
			Player.Spectating = false;
		}

		private Vector3 GetPos()
		{
			angleZ -= RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * Sensitivity; //-- around Z axis (left / right)
			angleY += RAGE.Game.Pad.GetDisabledControlNormal(1, 2) * Sensitivity; //-- up / down
			radius += (RAGE.Game.Pad.GetDisabledControlNormal(1, 14) + RAGE.Game.Pad.GetDisabledControlNormal(1, 15) * -1) * Sensitivity; //-- zoom
																																		  //-- limit up / down angle to 90°
			if (angleY > 1.5f) angleY = 1.5f;
			else if (angleY < -1.5f) angleY = -1.5f;

			Vector3 pCoords = SpecPlayer.Position;

			Vector3 behindCam = new Vector3(
				(float)(pCoords.X + ((Math.Cos(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Cos(angleZ))) / 2 * (radius + 0.5f)),
				(float)(pCoords.Y + ((Math.Sin(angleZ) * Math.Cos(angleY)) + (Math.Cos(angleY) * Math.Sin(angleZ))) / 2 * (radius + 0.5f)),
				(float)(pCoords.Z + Math.Sin(angleY) * (radius + 0.5f))
			);
			int rayHandle = RAGE.Game.Shapetest.StartShapeTestRay(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, behindCam.X, behindCam.Y, behindCam.Z, -1, SpecPlayer.Handle, 0);

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
				(float)(Math.Sin(angleY) * maxRadius)
			);

			Vector3 pos = new Vector3(
				pCoords.X + offset.X,
				pCoords.Y + offset.Y,
				pCoords.Z + offset.Z
			);

			return pos;
		}

		private static float Vdist(float x, float y, float z, Vector3 v2)
		{
			return v2.DistanceTo(new Vector3(x, y, z));
		}
	}
}
