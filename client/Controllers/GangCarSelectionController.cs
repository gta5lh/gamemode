﻿using System;
using System.Collections.Generic;
using System.Linq;
using GamemodeClient.Models;
using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Game;
using RAGE.Ui;
using static RAGE.Events;

namespace GamemodeClient.Controllers
{
	public class GangCarSelectionData
	{
		public GangCarSelectionData(Vector3 cameraPosition, Vector3 cameraRotation)
		{
			CameraPosition = cameraPosition;
			CameraRotation = cameraRotation;
		}

		public Vector3 CameraPosition { get; set; }

		public Vector3 CameraRotation { get; set; }
	}

	public class GangCarSelectionController : Events.Script
	{
		const float Sensitivity = 10.0f;

		private Spawn VehiclePos;
		private int VehicleColor;
		private int GangId;
		private int PlayerRank;

		private int Camera;
		private RAGE.Elements.Vehicle Vehicle;
		private int CurVeh = 1;

		private float angleZ = 0.0f;

		private static readonly Dictionary<int, GangCarSelectionData> GangCarSelectionDataByGangId = new Dictionary<int, GangCarSelectionData>()
		{
			{ 1, new GangCarSelectionData(new Vector3(499.03f, -1331.75f, 31.19f), new Vector3(-12.76f, 0, 100.82f))},
			{ 2, new GangCarSelectionData(new Vector3(105.30f, -1942.38f, 23.10f), new Vector3(-14.88f, 0, 156f))},
			{ 3, new GangCarSelectionData(new Vector3(-22.065f, -1464.6f, 32.72f), new Vector3(-14.76f, 0, 1.5f))},
			{ 4, new GangCarSelectionData(new Vector3(325.47f, -2030.21f, 22.6f), new Vector3(-13.16f, 0, 63.30f))},
			{ 5, new GangCarSelectionData(new Vector3(1367.528f, -1528.126f, 58.426f), new Vector3(-8.21f, 0, -57.74f))},
		};

		private List<GangVehicle> GangVehicles;
		private List<uint> VehModelsAvail;

		private bool canInteractWithMenu;

		private string MenuPath = $"package://cs_packages/gamemode/Frontend/Gang/Car/index.html";
		private HtmlWindow? Menu;

		public GangCarSelectionController()
		{
			// Common
			Events.Add("DisplayGangCarSelectionMenu", this.OnDisplayGangCarSelectionMenu);
			Events.Add("SetGangVehicles", this.OnSetGangVehicles);

			Input.Bind(RAGE.Ui.VirtualKeys.Left, true, Left);
			Input.Bind(RAGE.Ui.VirtualKeys.Right, true, Right);
			Input.Bind(RAGE.Ui.VirtualKeys.Return, true, Enter);

			Events.Tick += OnTick;
			Events.OnPlayerDeath += OnPlayerDeath;
		}

		public void Left()
		{
			if (Vehicle == null || CurVeh == 0) return;
			CurVeh--;
			Vehicle.Model = VehModelsAvail[CurVeh];
			Vehicle.SetOnGroundProperly(0);
		}

		public void Right()
		{
			if (Vehicle == null || CurVeh == VehModelsAvail.Count - 1) return;
			CurVeh++;
			Vehicle.Model = VehModelsAvail[CurVeh];
			Vehicle.SetOnGroundProperly(0);
		}

		public void Enter()
		{
			if (Vehicle == null) return;

			Events.CallRemote("PlayerSelectedGangCar", Vehicle.Model);
			OnExitKeyPressed();
		}

		private float GetRot()
		{
			angleZ += RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * Sensitivity; //-- around Z axis (left / right)
			return angleZ;
		}

		public void OnTick(List<Events.TickNametagData> nametags)
		{
			if (this.Menu == null) return;
			RAGE.Game.Player.DisablePlayerFiring(true);

			if (!Input.IsDown(RAGE.Ui.VirtualKeys.LeftButton)) return;

			Vehicle.Position = this.VehiclePos.Position;
			Vehicle.SetRotation(0, 0, GetRot(), 1, true);
			Vehicle.SetOnGroundProperly(0);
		}

		private void OnDisplayGangCarSelectionMenu(object[] args)
		{
			this.canInteractWithMenu = (bool)args[0];
			HelpPopUpController.Instance.SetEnabled(this.canInteractWithMenu);
			HelpPopUpController.InteractKeyPressed = this.OnInteractKeyPressed;
			HelpPopUpController.ExitKeyPressed = this.OnExitKeyPressed;

			if (!this.canInteractWithMenu)
			{
				HelpPopUpController.InteractKeyPressed = null;
				HelpPopUpController.ExitKeyPressed = null;
				return;
			}

			this.VehiclePos = ((JObject)args[1]).ToObject<Models.Spawn>();
			this.VehicleColor = (int)args[2];
			this.GangId = (int)args[3];
			this.PlayerRank = (int)args[4];
		}

		public async void OnInteractKeyPressed()
		{
			if (Cursor.Visible || this.Menu != null) return;

			uint id = Convert.ToUInt32(await Events.CallRemoteProc("SetOwnDimension"));

			this.Menu = Controllers.Menu.Open(this.canInteractWithMenu, this.Menu, this.MenuPath, true, true);

			GangCarSelectionData gangCarSelectionData = GangCarSelectionDataByGangId[this.GangId];

			Player.CurrentPlayer.FreezePosition(true);
			Player.CurrentPlayer.SetVisible(false, false);
			RAGE.Game.Ui.DisplayRadar(false);
			VehModelsAvail = GangVehicles.Where(x => x.Rank <= this.PlayerRank).Select(x => x.Model).ToList();
			if (CurVeh >= VehModelsAvail.Count) CurVeh = VehModelsAvail.Count - 1;
			Vehicle = new RAGE.Elements.Vehicle(GangVehicles[CurVeh].Model, this.VehiclePos.Position, this.VehiclePos.Heading, "", 255, false, this.VehicleColor, this.VehicleColor, id);
			Vehicle.SetOnGroundProperly(0);

			Camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), gangCarSelectionData.CameraPosition.X, gangCarSelectionData.CameraPosition.Y, gangCarSelectionData.CameraPosition.Z, gangCarSelectionData.CameraRotation.X, gangCarSelectionData.CameraRotation.Y, gangCarSelectionData.CameraRotation.Z, 50, true, 2);
			Cam.SetCamActive(Camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);

		}

		private void OnExitKeyPressed()
		{
			if (!Controllers.Menu.Close(ref this.Menu)) return;

			Vehicle.Destroy();
			Vehicle = null;
			Player.CurrentPlayer.FreezePosition(false);
			Player.CurrentPlayer.SetVisible(true, false);
			RAGE.Game.Ui.DisplayRadar(true);
			Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Cam.DestroyCam(Camera, false);
			Events.CallRemote("SetServerDimension");
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.canInteractWithMenu = false;
			if (Controllers.Menu.Close(ref this.Menu))
			{
				Events.CallRemote("SetServerDimension");
			}
		}


		public void OnSetGangVehicles(object[] args)
		{
			this.GangVehicles = ((JArray)args[0]).ToObject<List<GangVehicle>>();
		}
	}

	public class GangVehicle
	{
		public GangVehicle(uint model, byte rank)
		{
			Model = model;
			Rank = rank;
		}

		public uint Model { get; set; }

		public byte Rank { get; set; }
	}
}
