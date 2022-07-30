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
		private readonly float sensitivity = 0.15f;

		private int camera;
		private int specId = -1;
		private RAGE.Elements.Player specPlayer = null!;

		private float angleY = 0.0f;
		private float angleZ = 0.0f;
		private float radius = 6.0f;

		private DateTime nextUpdateTime = DateTime.MinValue;

		private const short UpdateTimeIntervalMilliseconds = 500;

		public Spectate()
		{
			Events.Tick += this.OnTick;
			Events.Add("spectate", this.Spec);
			Events.Add("spectateStop", this.DisableSpec);
		}

		public void Spec(object[] args)
		{
			this.specId = (int)args[0];
			Game.Player.Models.Player.Spectating = true;
			Game.Player.Models.Player.CurrentPlayer.FreezePosition(true);
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (this.specPlayer == null && Game.Player.Models.Player.Spectating)
			{
				if (DateTime.UtcNow < this.nextUpdateTime)
				{
					return;
				}

				foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
				{
					if (player.RemoteId != this.specId)
					{
						continue;
					}

					this.EnableSpec(player);
					return;
				}

				this.nextUpdateTime = DateTime.UtcNow.AddMilliseconds(UpdateTimeIntervalMilliseconds);
				return;
			}

			if (this.specPlayer == null)
			{
				return;
			}

			RAGE.Game.Pad.DisableControlAction(2, 16, true);

			if (!this.specPlayer.Exists)
			{
				Events.CallRemote("TrySpectate", this.specPlayer.RemoteId);
				this.specPlayer = null!;
				return;
			}

			this.UpdateCameraPosition();
		}

		private void UpdateCameraPosition()
		{
			RAGE.Elements.Player.LocalPlayer.Position = this.specPlayer.Position + new Vector3(0, 0, 2);

			Vector3 newPos = this.GetPos();

			Cam.SetCamCoord(this.camera, newPos.X, newPos.Y, newPos.Z);
			Cam.PointCamAtCoord(this.camera, this.specPlayer.Position.X, this.specPlayer.Position.Y, this.specPlayer.Position.Z + 0.5f);
		}

		private void EnableSpec(RAGE.Elements.Player player)
		{
			this.specPlayer = player;
			this.specId = -1;

			if (!Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetInvincible(true);
				Game.Player.Models.Player.CurrentPlayer.SetAlpha(0, false);
				Game.Player.Models.Player.CurrentPlayer.SetVisible(false, false);
				Game.Player.Models.Player.CurrentPlayer.SetCollision(false, false);
			}

			if (Cam.DoesCamExist(this.camera))
			{
				Cam.RenderScriptCams(false, false, 0, true, false, 0);
				Cam.DestroyCam(this.camera, false);
			}

			this.camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey(Constants.CameraName), this.specPlayer.Position.X, this.specPlayer.Position.Y, this.specPlayer.Position.Z, 0, 0, 0, Cam.GetGameplayCamFov(), true, 2);
			Cam.SetCamActive(this.camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);
			this.UpdateCameraPosition();

			Game.Player.Models.Player.CurrentPlayer.SetCurrentWeaponVisible(false, true, true, true);
			Game.Player.Models.Player.Spectating = true;
		}

		private void DisableSpec(object[] args)
		{
			if (!Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetCurrentWeaponVisible(true, false, true, true);
				Game.Player.Models.Player.CurrentPlayer.SetAlpha(255, false);
				Game.Player.Models.Player.CurrentPlayer.SetVisible(true, false);
			}

			Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Cam.DestroyCam(this.camera, false);
			Game.Player.Models.Player.CurrentPlayer.FreezePosition(false);
			Game.Player.Models.Player.CurrentPlayer.SetCollision(true, false);

			if (!Game.Player.Models.Player.GodmodEnabled && !Game.Player.Models.Player.InvisibilityEnabled)
			{
				Game.Player.Models.Player.CurrentPlayer.SetInvincible(false);
			}

			this.specPlayer = null;
			Game.Player.Models.Player.Spectating = false;
		}

		private Vector3 GetPos()
		{
			this.angleZ -= RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * this.sensitivity; //-- around Z axis (left / right)
			this.angleY += RAGE.Game.Pad.GetDisabledControlNormal(1, 2) * this.sensitivity; //-- up / down
			this.radius += (RAGE.Game.Pad.GetDisabledControlNormal(1, 14) + (RAGE.Game.Pad.GetDisabledControlNormal(1, 15) * -1)) * this.sensitivity; //-- zoom

																																					//-- limit up / down angle to 90°
			if (this.angleY > 1.5f)
			{
				this.angleY = 1.5f;
			}
			else if (this.angleY < -1.5f)
			{
				this.angleY = -1.5f;
			}

			Vector3 pCoords = this.specPlayer.Position;

			Vector3 behindCam = new Vector3(
				(float)(pCoords.X + (((Math.Cos(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Cos(this.angleZ))) / 2 * (this.radius + 0.5f))),
				(float)(pCoords.Y + (((Math.Sin(this.angleZ) * Math.Cos(this.angleY)) + (Math.Cos(this.angleY) * Math.Sin(this.angleZ))) / 2 * (this.radius + 0.5f))),
				(float)(pCoords.Z + (Math.Sin(this.angleY) * (this.radius + 0.5f))));

			int rayHandle = RAGE.Game.Shapetest.StartShapeTestRay(pCoords.X, pCoords.Y, pCoords.Z + 0.5f, behindCam.X, behindCam.Y, behindCam.Z, -1, this.specPlayer.Handle, 0);

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
