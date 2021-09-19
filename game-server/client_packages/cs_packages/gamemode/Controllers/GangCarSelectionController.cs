namespace GamemodeClient.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using GamemodeClient.Models;
	using GamemodeCommon.Models;
	using Newtonsoft.Json.Linq;
	using RAGE;
	using RAGE.Game;
	using RAGE.Ui;
	using static RAGE.Events;
	using static GamemodeClient.Controllers.Cef.Cef;

	public class GangCarSelectionController : Events.Script
	{
		const float Sensitivity = 10.0f;

		private Spawn VehiclePos;
		private int VehicleColor;
		private int GangId;
		private long PlayerRank;

		private int Camera;
		private RAGE.Elements.Vehicle Vehicle;
		private int CurVeh = 1;

		private float angleZ = 0.0f;
		private bool isInCarSelection = false;

		private static readonly Dictionary<int, GangCarSelectionData> GangCarSelectionDataByGangId = new Dictionary<int, GangCarSelectionData>()
		{
			{ 1, new GangCarSelectionData(new Vector3(499.03f, -1331.75f, 31.19f), new Vector3(-12.76f, 0, 100.82f)) },
			{ 2, new GangCarSelectionData(new Vector3(105.30f, -1942.38f, 23.10f), new Vector3(-14.88f, 0, 156f)) },
			{ 3, new GangCarSelectionData(new Vector3(-22.065f, -1464.6f, 32.72f), new Vector3(-14.76f, 0, 1.5f)) },
			{ 4, new GangCarSelectionData(new Vector3(325.47f, -2030.21f, 22.6f), new Vector3(-13.16f, 0, 63.30f)) },
			{ 5, new GangCarSelectionData(new Vector3(1367.528f, -1528.126f, 58.426f), new Vector3(-8.21f, 0, -57.74f)) },
		};

		private List<GangVehicle> GangVehicles;
		private List<uint> VehModelsAvail;

		private bool canInteractWithMenu;

		public GangCarSelectionController()
		{
			// Common
			Events.Add("DisplayGangCarSelectionMenu", this.OnDisplayGangCarSelectionMenu);
			Events.Add("SetGangVehicles", this.OnSetGangVehicles);
			Events.Add("CarSelected", this.OnCarSelected);
			Events.Add("CloseCarPark", this.OnCloseCarPark);
			Events.Add("TakeCar", this.OnTakeCar);

			Input.Bind(RAGE.Ui.VirtualKeys.Left, true, this.Left);
			Input.Bind(RAGE.Ui.VirtualKeys.Right, true, this.Right);
			Input.Bind(RAGE.Ui.VirtualKeys.Return, true, this.Enter);

			Events.Tick += this.OnTick;
			Events.OnPlayerDeath += this.OnPlayerDeath;
		}

		public void Left()
		{
			if (this.Vehicle == null || this.CurVeh == 0)
			{
				return;
			}

			this.CurVeh--;
			UpdateSelectedCar(this.CurVeh);
			this.Vehicle.Model = this.VehModelsAvail[this.CurVeh];
			this.Vehicle.SetOnGroundProperly(0);
		}

		public void Right()
		{
			if (this.Vehicle == null || this.CurVeh == this.VehModelsAvail.Count - 1)
			{
				return;
			}

			this.CurVeh++;
			UpdateSelectedCar(this.CurVeh);
			this.Vehicle.Model = this.VehModelsAvail[this.CurVeh];
			this.Vehicle.SetOnGroundProperly(0);
		}

		public async void Enter()
		{
			if (this.Vehicle == null)
			{
				return;
			}

			await Events.CallRemoteProc("PlayerSelectedGangCar", this.Vehicle.Model);
			Player.CurrentPlayer.Vehicle.SetOnGroundProperly(0);
			this.OnExitKeyPressed();
		}

		private float GetRot()
		{
			this.angleZ += RAGE.Game.Pad.GetDisabledControlNormal(1, 1) * Sensitivity; //-- around Z axis (left / right)
			return this.angleZ;
		}

		public void OnTick(List<Events.TickNametagData> nametags)
		{
			if (!this.isInCarSelection)
			{
				return;
			}

			RAGE.Game.Player.DisablePlayerFiring(true);

			if (!Input.IsDown(RAGE.Ui.VirtualKeys.RightButton))
			{
				return;
			}

			this.Vehicle.Position = this.VehiclePos.Position;
			this.Vehicle.SetRotation(0, 0, this.GetRot(), 1, true);
			this.Vehicle.SetOnGroundProperly(0);
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
			this.PlayerRank = (long)args[4];
		}

		public async void OnInteractKeyPressed()
		{
			if (Cursor.Visible || !this.canInteractWithMenu)
			{
				return;
			}

			uint id = Convert.ToUInt32(await Events.CallRemoteProc("SetOwnDimension"));

			ShowCarPark(new ShowCarPark(this.PlayerRank, this.CurVeh));

			GangCarSelectionData gangCarSelectionData = GangCarSelectionDataByGangId[this.GangId];

			Player.CurrentPlayer.FreezePosition(true);
			Player.CurrentPlayer.SetVisible(false, false);
			RAGE.Game.Ui.DisplayRadar(false);
			this.VehModelsAvail = this.GangVehicles.Where(x => x.Rank <= this.PlayerRank).Select(x => x.Model).ToList();
			if (this.CurVeh >= this.VehModelsAvail.Count)
			{
				this.CurVeh = this.VehModelsAvail.Count - 1;
			}

			this.Vehicle = new RAGE.Elements.Vehicle(this.GangVehicles[this.CurVeh].Model, this.VehiclePos.Position, this.VehiclePos.Heading, string.Empty, 255, false, this.VehicleColor, this.VehicleColor, id);
			this.Vehicle.SetRotation(0, 0, this.VehiclePos.Heading, 1, true);
			this.Vehicle.SetOnGroundProperly(0);
			this.angleZ = this.VehiclePos.Heading;

			this.Camera = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), gangCarSelectionData.CameraPosition.X, gangCarSelectionData.CameraPosition.Y, gangCarSelectionData.CameraPosition.Z, gangCarSelectionData.CameraRotation.X, gangCarSelectionData.CameraRotation.Y, gangCarSelectionData.CameraRotation.Z, 50, true, 2);
			Cam.SetCamActive(this.Camera, true);
			Cam.RenderScriptCams(true, false, 0, true, false, 0);
			this.isInCarSelection = true;
		}

		private void OnExitKeyPressed()
		{
			this.Vehicle.Destroy();
			this.Vehicle = null;
			this.isInCarSelection = false;
			Player.CurrentPlayer.FreezePosition(false);
			Player.CurrentPlayer.SetVisible(true, false);
			RAGE.Game.Ui.DisplayRadar(true);
			Cam.RenderScriptCams(false, false, 0, true, false, 0);
			Cam.DestroyCam(this.Camera, false);
			Events.CallRemote("SetServerDimension");
			CloseCarPark();
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.canInteractWithMenu = false;
			this.isInCarSelection = false;
			Events.CallRemote("SetServerDimension");
		}

		public void OnSetGangVehicles(object[] args)
		{
			this.GangVehicles = ((JArray)args[0]).ToObject<List<GangVehicle>>();
		}

		public void OnCarSelected(object[] args)
		{
			this.CurVeh = (int)args[0];
			this.Vehicle.Model = this.VehModelsAvail[this.CurVeh];
			this.Vehicle.SetOnGroundProperly(0);
		}

		public void OnCloseCarPark(object[] args)
		{
			this.OnExitKeyPressed();
		}

		public void OnTakeCar(object[] args)
		{
			this.Enter();
		}
	}
}
