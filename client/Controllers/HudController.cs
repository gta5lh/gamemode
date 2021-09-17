// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using GamemodeCommon.Authentication.Models;
	using static GamemodeClient.Controllers.Cef.Cef;
	using Newtonsoft.Json;
	using RAGE;
	using RAGE.Ui;
	using System;
	using GamemodeClient.Services;
	using RAGE.Elements;
	using GamemodeClient.Utils;
	using Newtonsoft.Json.Linq;
	using GamemodeClient.Models;

	public partial class HudController : Events.Script
	{
		private long Money = 0;
		private int Hours = 0;
		private int Minutes = 0;
		private int Day = 0;
		private int Month = 0;
		private bool IsInZone = false;
		private string ZoneColor = "green";
		private Stats stats = new Stats(0, 0, 0, 0, 0);
		private DateTime finishTime;
		private long targetFractionId;
		private int gangWarState = 0;

		private bool HelpMenuEnabled = true;

		public HudController()
		{
			AuthenticationController.playerAuthenticatedEvent += this.OnPlayerAuthenticated;
			VoiceChatController.playerVoiceStateChangedEvent += this.OnPlayerVoiceStateChanged;
			DisableUIController.disableUIStateChangedEvent += this.OnDisableUIStateChanged;
			disableUIStateChangedEvent += this.OnDisableUIStateChanged;
			SafeZoneController.playerSafeZoneEvent += this.OnPlayerSafeZoneEvent;

			Events.OnPlayerEnterVehicle += this.OnPlayerEnterVehicle;
			Events.OnPlayerLeaveVehicle += this.OnPlayerLeaveVehicle;
			Events.Tick += this.Tick;
			Events.Tick += this.Speedometer;
			Events.Add("MoneyUpdated", this.OnMoneyUpdated);
			Events.Add("SetZoneState", this.OnSetZoneState);
			Events.Add("InitGangWarUI", this.OnInitGangWarUI);
			Events.Add("StartGangWarUI", this.OnStartGangWarUI);
			Events.Add("CloseGangWarUI", this.OnCloseGangWarUI);
			Events.Add("UpdateGangWarStats", this.OnUpdateGangWarStats);

			Events.AddDataHandler(GamemodeCommon.Models.Data.DataKey.CurrentTime, this.OnTimeUpdated);

			RAGE.Input.Bind(VirtualKeys.F6, false, this.OnHideHelpKeyPressed);

			// Experience
			Events.Add("ExperienceChanged", this.OnExperienceChanged);
			Events.Add("RankedUp", this.OnRankedUp);
			Events.Add("RankedDown", this.OnRankedDown);
		}

		private void OnUpdateGangWarStats(object[] args)
		{
			long ballas = (long)args[0];
			long bloods = (long)args[1];
			long marabunta = (long)args[2];
			long families = (long)args[3];
			long vagos = (long)args[4];

			this.stats = new Stats(ballas, bloods, marabunta, families, vagos);

			UpdateStats(this.stats);
		}

		private void OnInitGangWarUI(object[] args)
		{
			this.finishTime = JsonConvert.DeserializeObject<DateTime>((string)args[0]);
			this.gangWarState = 1;
			InitGangWar(this.remainingMs());
		}

		private void OnStartGangWarUI(object[] args)
		{
			this.finishTime = JsonConvert.DeserializeObject<DateTime>((string)args[0]);
			this.targetFractionId = (long)args[1];
			this.gangWarState = 2;
			this.stats = new Stats(0, 0, 0, 0, 0);
			StartGangWar(new StartGangWar(this.remainingMs(), targetFractionId));
		}

		private void OnCloseGangWarUI(object[] args)
		{
			this.gangWarState = 0;
			HideCapt();
		}

		private double remainingMs()
		{
			DateTime startTime = DateTime.UtcNow;
			return (this.finishTime - startTime).TotalMilliseconds;
		}

		private void OnHideHelpKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			this.HelpMenuEnabled = !this.HelpMenuEnabled;
			if (this.HelpMenuEnabled)
			{
				ShowHelpMenu();
			}
			else
			{
				HideHelpMenu();
			}
		}

		private void OnTimeUpdated(Entity entity, object arg, object oldArg)
		{
			this.SetTime(arg);
		}

		private void OnMoneyUpdated(object[] request)
		{
			this.Money = (long)request[0];
			UpdateMoney(this.Money);
		}

		private void OnSetZoneState(object[] request)
		{
			this.IsInZone = (bool)request[0];
			this.ZoneColor = (string)request[1];

			SetZoneState(this.IsInZone, this.ZoneColor);
		}

		private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
		{
			ShowSpeedometer();
		}

		private void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)
		{
			HideSpeedometer();
		}

		public void OnPlayerSafeZoneEvent(bool enabled)
		{
			this.IsInZone = enabled;
			this.ZoneColor = "green";
			SetZoneState(this.IsInZone, this.ZoneColor);
		}

		public void OnDisableUIStateChanged(bool enabled)
		{
			if (enabled)
			{
				ShowHud();
				UpdateMoney(this.Money);
				UpdateOnline();
				UpdateTime(this.Hours, this.Minutes, this.Day, this.Month);
				SetZoneState(this.IsInZone, this.ZoneColor);
				this.SetCurrentExperience();
				SetXAndY();

				if (this.gangWarState == 1)
				{
					InitGangWar(this.remainingMs());
				}
				else if (this.gangWarState == 2)
				{
					StartGangWar(new StartGangWar(this.remainingMs(), this.targetFractionId));
					UpdateStats(this.stats);
				}

				if (!HelpMenuEnabled)
				{
					HideHelpMenu();
				}

				if (Player.IsInVehicle())
				{
					ShowSpeedometer();
				}
			}
			else
			{
				HideHud();
			}
		}

		public void OnPlayerAuthenticated()
		{
			ShowHud();
			UpdateMoney(this.Money);
			UpdateOnline();
			this.SetCurrentExperience();
			SetXAndY();

			DummyEntity? timeSyncDummyEntity = DummyEntityUtil.GetByTypeID(0);
			if (timeSyncDummyEntity != null)
			{
				this.SetTime(timeSyncDummyEntity.GetSharedData(GamemodeCommon.Models.Data.DataKey.CurrentTime));
			}
		}

		public void OnPlayerVoiceStateChanged(bool enabled)
		{
			if (enabled)
			{
				ShowVoice();
			}
			else
			{
				HideVoice();
			}
		}

		private DateTime NextOnlineUpdateTime = DateTime.MinValue;
		private const byte UpdateOnlineTimeIntervalSeconds = 1;
		private const int HudWeapon = 2;
		private const int HudCash = 3;
		private const int HudVehicleName = 6;
		private const int HudAreaName = 7;
		private const int HudVehicleClass = 8;
		private const int HudStreetName = 9;
		private const int WeaponWheelStats = 20;

		private void Speedometer(List<Events.TickNametagData> nametags)
		{
			if (!Player.IsInVehicle())
			{
				return;
			}

			UpdateSpeedometer(Speed.GetPlayerRealSpeed(Player.CurrentPlayer));
		}

		private void Tick(List<Events.TickNametagData> nametags)
		{
			this.HideHudComponentIfActive(HudWeapon);
			this.HideHudComponentIfActive(HudCash);
			this.HideHudComponentIfActive(HudAreaName);
			this.HideHudComponentIfActive(HudStreetName);
			this.HideHudComponentIfActive(HudVehicleName);
			this.HideHudComponentIfActive(HudVehicleClass);

			if (IndexCef.Active && (RAGE.Game.Ui.IsHudComponentActive(WeaponWheelStats) || RAGE.Game.Ui.IsPauseMenuActive()))
			{
				IndexCef.Active = false;
			}
			else if (!IndexCef.Active && (!RAGE.Game.Ui.IsHudComponentActive(WeaponWheelStats) && !RAGE.Game.Ui.IsPauseMenuActive()))
			{
				IndexCef.Active = true;
			}

			if (!IndexCef.Active || DateTime.UtcNow < this.NextOnlineUpdateTime)
			{
				return;
			}

			UpdateOnline();
			this.NextOnlineUpdateTime = DateTime.UtcNow.AddSeconds(UpdateOnlineTimeIntervalSeconds);
		}

		private void HideHudComponentIfActive(int hud)
		{
			if (RAGE.Game.Ui.IsHudComponentActive(hud))
			{
				RAGE.Game.Ui.HideHudComponentThisFrame(hud);
			}
		}

		private void SetTime(object arg)
		{
			Dictionary<string, object> data = ((JObject)arg).ToObject<Dictionary<string, object>>();
			this.Hours = Convert.ToInt32(data["hours"]);
			this.Minutes = Convert.ToInt32(data["minutes"]);
			this.Day = Convert.ToInt32(data["day"]);
			this.Month = Convert.ToInt32(data["month"]);
			UpdateTime(this.Hours, this.Minutes, this.Day, this.Month);
		}
	}
}
