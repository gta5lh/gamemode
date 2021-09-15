// <copyright file="SpectateController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Elements;
	using System;
	using System.Collections.Generic;
	using Cam = RAGE.Game.Cam;

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
			Events.Tick += this.OnTick;
			Events.Add("spectate", this.Spec);
			Events.Add("spectateStop", this.DisableSpec);
		}

		public void Spec(object[] args)
		{
			this.SpecId = (int)args[0];
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
					if (player.RemoteId != this.SpecId)
					{
						continue;
					}

					this.EnableSpec(player);
					return;
				}

				this.NextUpdateTime = DateTime.UtcNow.AddMilliseconds(UpdateTimeIntervalMilliseconds);
				return;
			}

			if (this.SpecPlayer == null)
			{
				return;
			}

			RAGE.Game.Pad.DisableControlAction(2, 16, true);

			if (!SpecPlayer.Exists)
			{
				Events.CallRemote("TrySpectate", this.SpecPlayer.RemoteId);
				this.SpecPlayer = null;
				return;
			}

			UpdateCameraPosition();
		}

		private void UpdateCameraPosition()
		{
			RAGE.Elements.Player.LocalPlayer.Position = this.SpecPlayer.Position + new Vector3(0, 0, 2);

			Vector3 newPos = this.GetPos();

			Cam.SetCamCoord(this.Camera, newPos.X, newPos.Y, newPos.Z);
			Cam.PointCamAtCoord(this.Camera, this.SpecPlayer.Position.X, this.SpecPlayer.Position.Y, this.SpecPlayer.Position.Z + 0.5f);
		}

		private void EnableSpec(RAGE.Elements.Player player)
		{
			this.SpecPlayer = player;
			this.SpecId = -1;

			if (!Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(true);
				Player.CurrentPlayer.SetAlpha(0, false);
				Player.CurrentPlayer.SetVisible(false, false);
				Player.CurrentPlayer.SetCollision(false, false);
			}

			if (Cam.DoesCamExist(this.Camera))
			{
				Cam.RenderScriptCams(false, false, 0, true, false, 0);
				Cam.DestroyCam(this.Camera, false);
			}

			this.Camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey(Constants.CameraName), this.SpecPlayer.Position.X, this.SpecPlayer.Position.Y, this.SpecPlayer.Position.Z, 0, 0, 0, Cam.GetGameplayCamFov(), true, 2);
			Cam.SetCamActive(this.Camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);
			this.UpdateCameraPosition();

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
			Cam.DestroyCam(this.Camera, false);
			Player.CurrentPlayer.FreezePosition(false);
			Player.CurrentPlayer.SetCollision(true, false);

			if (!Player.GodmodEnabled && !Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(false);
			}

			this.SpecPlayer = null;
			Player.Spectating = false;
		}

		private Vector3 GetPos()
		{
			this.angleZ -= RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * this.Sensitivity; //-- around Z axis (left / right)
			this.angleY += RAGE.Game.Pad.GetDisabledControlNormal(1, 2) * this.Sensitivity; //-- up / down
			this.radius += (RAGE.Game.Pad.GetDisabledControlNormal(1, 14) + RAGE.Game.Pad.GetDisabledControlNormal(1, 15) * -1) * this.Sensitivity; //-- zoom
																																					//-- limit up / down angle to 90°
			if (this.angleY > 1.5f)
			{
				this.angleY = 1.5f;
			}
			else if (this.angleY < -1.5f)
			{
				this.angleY = -1.5f;
			}

			Vector3 pCoords = this.SpecPlayer.Position;

			Vector3 behindCam = new Vector3(
				(float)(pCoords.X + ((Math.Cos(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Cos(this.angleZ))) / 2 * (this.radius + 0.5f)),
				(float)(pCoords.Y + ((Math.Sin(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Sin(this.angleZ))) / 2 * (this.radius + 0.5f)),
				(float)(pCoords.Z + Math.Sin(this.angleY) * (this.radius + 0.5f)));

			int rayHandle = RAGE.Game.Shapetest.StartShapeTestRay(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, behindCam.X, behindCam.Y, behindCam.Z, -1, this.SpecPlayer.Handle, 0);

			int hitBool = 0;
			Vector3 hitCoords = new Vector3();
			Vector3 surfaceNormal = new Vector3();
			int entityHit = 0;

			RAGE.Game.Shapetest.GetShapeTestResult(rayHandle, ref hitBool, hitCoords, surfaceNormal, ref entityHit);

			float maxRadius = this.radius;
			if (Convert.ToBoolean(hitBool) && Vdist(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, hitCoords) < this.radius + 0.5f)
			{
				maxRadius = Vdist(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, hitCoords);
			}

			Vector3 offset = new Vector3(
				(float)(((Math.Cos(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Cos(this.angleZ))) / 2 * maxRadius),
				(float)(((Math.Sin(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Sin(this.angleZ))) / 2 * maxRadius),
				(float)(Math.Sin(this.angleY) * maxRadius));

			Vector3 pos = new Vector3(pCoords.X + offset.X, pCoords.Y + offset.Y, pCoords.Z + offset.Z);

			return pos;
		}

		private static float Vdist(float x, float y, float z, Vector3 v2)
		{
			return v2.DistanceTo(new Vector3(x, y, z));
		}
	}
}
