﻿// <copyright file="Noclip.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.Admin
{
	using System;
	using System.Collections.Generic;
	using GamemodeClient.Game.Player.Models;
	using GamemodeCommon.Models.Data;
	using RAGE;
	using RAGE.Ui;

	public class Noclip : Events.Script
	{
		private static int noclipCamera;

		static Noclip()
		{
			Input.Bind(VirtualKeys.F10, false, OnNoclipKeyPressed);
			Events.Tick += OnTick;
		}

		public static Vector3 GetDirectionByRotation(Vector3 rotation)
		{
			float num = rotation.Z * 0.0174532924f;
			float num2 = rotation.X * 0.0174532924f;
			float num3 = MathF.Abs(MathF.Cos(num2));
			return new Vector3 { X = -MathF.Sin(num) * num3, Y = MathF.Cos(num) * num3, Z = MathF.Sin(num2) };
		}

		private static Vector3 CrossProduct(Vector3 vector1, Vector3 vector2)
		{
			return new Vector3(0, 0, 0)
			{
				X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y),
				Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z),
				Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X),
			};
		}

		private static void OnTick(List<Events.TickNametagData> nametags)
		{
			if (!Player.NoclipEnabled || Cursor.Visible)
			{
				return;
			}

			var fastMult = 1f;
			var slowMult = 1f;
			if (Input.IsDown(VirtualKeys.Shift))
			{
				fastMult = 3f;
			}
			else if (Input.IsDown(VirtualKeys.LeftControl))
			{
				slowMult = 0.2f;
			}

			var leftAxisX = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.ScriptLeftAxisX);
			var leftAxisY = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.ScriptLeftAxisY);
			var rightAxisX = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.ScriptRightAxisX);
			var rightAxisY = RAGE.Game.Pad.GetDisabledControlNormal(0, (int)RAGE.Game.Control.ScriptRightAxisY);
			var currentPosition = RAGE.Game.Cam.GetCamCoord(noclipCamera);
			var currentRotation = RAGE.Game.Cam.GetCamRot(noclipCamera, 2);
			var currentDirection = GetDirectionByRotation(currentRotation);
			Vector3 vector = new Vector3(0, 0, 0)
			{
				X = currentDirection.X * leftAxisY * fastMult * slowMult,
				Y = currentDirection.Y * leftAxisY * fastMult * slowMult,
				Z = currentDirection.Z * leftAxisY * fastMult * slowMult,
			};

			var upVector = new Vector3(0, 0, 1);
			Vector3 rightVector = CrossProduct(currentDirection.Normalized, upVector.Normalized);

			rightVector.X *= leftAxisX * 0.5f;
			rightVector.Y *= leftAxisX * 0.5f;
			rightVector.Z *= leftAxisX * 0.5f;

			var upMovement = 0.0f;
			if (Input.IsDown(VirtualKeys.Q))
			{
				upMovement = 0.5f;
			}

			var downMovement = 0.0f;
			if (Input.IsDown(VirtualKeys.E))
			{
				downMovement = 0.5f;
			}

			Player.CurrentPlayer.Position = new Vector3(
				currentPosition.X + vector.X + 1,
				currentPosition.Y + vector.Y + 1,
				currentPosition.Z + vector.Z + 1);

			Player.CurrentPlayer.SetHeading(currentDirection.Z);
			RAGE.Game.Cam.SetCamCoord(
				noclipCamera,
				currentPosition.X - vector.X + rightVector.X,
				currentPosition.Y - vector.Y + rightVector.Y,
				currentPosition.Z - vector.Z + rightVector.Z + upMovement - downMovement);

			RAGE.Game.Cam.SetCamRot(
				noclipCamera,
				currentRotation.X + (rightAxisY * -5.0f),
				0.0f,
				currentRotation.Z + (rightAxisX * -5.0f),
				2);
		}

		private static void OnNoclipKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			if (Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin) == null || !(bool)Player.CurrentPlayer.GetSharedData(DataKey.IsAdmin))
			{
				return;
			}

			if (Player.Spectating)
			{
				Chat.Output("Нельзя использовать Noclip в режиме слежения");
				return;
			}

			Player.NoclipEnabled = !Player.NoclipEnabled;

			if (Player.NoclipEnabled)
			{
				EnableNoclip();
			}
			else
			{
				DisableNoclip();
			}

			string enabledString = Player.NoclipEnabled ? "включили" : "выключили";
			Chat.Output($"Вы {enabledString} noclip");
		}

		private static void EnableNoclip()
		{
			Player.NoclipEnabled = true;
			Events.CallRemote("SetNoclip", Player.NoclipEnabled);

			if (!Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(true);
				Player.CurrentPlayer.SetAlpha(0, false);
				Player.CurrentPlayer.SetVisible(false, false);
				Player.CurrentPlayer.SetCollision(false, false);
			}

			Vector3 playerPosition = Player.CurrentPlayer.Position;
			Vector3 playerRotation = RAGE.Game.Cam.GetGameplayCamRot(2);
			noclipCamera = RAGE.Game.Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey(Constants.CameraName), playerPosition.X, playerPosition.Y, playerPosition.Z, playerRotation.X, playerRotation.Y, playerRotation.Z, RAGE.Game.Cam.GetGameplayCamFov(), true, 2);
			RAGE.Game.Cam.SetCamActive(noclipCamera, true);
			RAGE.Game.Cam.RenderScriptCams(true, false, 0, true, false, 0);
			Player.CurrentPlayer.SetCurrentWeaponVisible(false, true, true, true);
			Player.CurrentPlayer.FreezePosition(true);
		}

		private static void DisableNoclip()
		{
			Player.NoclipEnabled = false;
			Events.CallRemote("SetNoclip", Player.NoclipEnabled);

			if (!Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetCurrentWeaponVisible(true, false, true, true);
				Player.CurrentPlayer.SetAlpha(255, false);
				Player.CurrentPlayer.SetVisible(true, false);
			}

			Player.CurrentPlayer.Position = RAGE.Game.Cam.GetCamCoord(noclipCamera);
			Player.CurrentPlayer.SetHeading(RAGE.Game.Cam.GetCamRot(noclipCamera, 2).Z);
			RAGE.Game.Cam.DestroyCam(noclipCamera, false);
			RAGE.Game.Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Player.CurrentPlayer.FreezePosition(false);
			Player.CurrentPlayer.SetCollision(true, false);

			if (!Player.GodmodEnabled && !Player.InvisibilityEnabled)
			{
				Player.CurrentPlayer.SetInvincible(false);
			}

			float groundZ = Player.CurrentPlayer.Position.Z;
			RAGE.Game.Misc.GetGroundZFor3dCoord(Player.CurrentPlayer.Position.X, Player.CurrentPlayer.Position.Y, Player.CurrentPlayer.Position.Z, ref groundZ, false);

			Vector3 newPosition = Player.CurrentPlayer.Position;
			newPosition.Z = groundZ;
			Player.CurrentPlayer.Position = newPosition;
		}
	}
}
