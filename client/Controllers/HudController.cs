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

	public class HudController : Events.Script
	{
		private long Money = 0;
		private int Hours = 0;
		private int Minutes = 0;
		private int Day = 0;
		private int Month = 0;

		private bool HelpMenuEnabled = true;

		public HudController()
		{
			AuthenticationController.playerAuthenticatedEvent += OnPlayerAuthenticated;
			VoiceChatController.playerVoiceStateChangedEvent += OnPlayerVoiceStateChanged;
			DisableUIController.disableUIStateChangedEvent += OnDisableUIStateChanged;

			Events.OnPlayerEnterVehicle += this.OnPlayerEnterVehicle;
			Events.OnPlayerLeaveVehicle += this.OnPlayerLeaveVehicle;
			Events.Tick += this.Tick;
			Events.Tick += this.Speedometer;
			Events.Add("MoneyUpdated", this.OnMoneyUpdated);

			Events.AddDataHandler(GamemodeCommon.Models.Data.DataKey.CurrentTime, this.OnTimeUpdated);

			RAGE.Input.Bind(VirtualKeys.F6, false, this.OnHideHelpKeyPressed);
		}

		private void OnHideHelpKeyPressed()
		{
			if (Cursor.Visible) return;

			HelpMenuEnabled = !HelpMenuEnabled;
			if (HelpMenuEnabled) ShowHelpMenu();
			else HideHelpMenu();
		}

		private void OnTimeUpdated(Entity entity, object arg, object oldArg)
		{
			SetTime(arg);
		}

		private void OnMoneyUpdated(object[] request)
		{
			Money = (long)request[0];
			UpdateMoney(Money);
		}


		private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
		{
			ShowSpeedometer();
		}

		private void OnPlayerLeaveVehicle(Vehicle vehicle, int seatId)
		{
			HideSpeedometer();
		}

		public void OnDisableUIStateChanged(bool enabled)
		{
			if (enabled)
			{
				ShowHud();
				UpdateMoney(Money);
				UpdateOnline();
				UpdateTime(Hours, Minutes, Day, Month);

				if (!HelpMenuEnabled) HideHelpMenu();
				if (Player.IsInVehicle()) ShowSpeedometer();
			}
			else HideHud();
		}

		public void OnPlayerAuthenticated()
		{
			ShowHud();
			UpdateMoney(Money);
			UpdateOnline();

			DummyEntity? timeSyncDummyEntity = DummyEntityUtil.GetByTypeID(0);
			if (timeSyncDummyEntity != null)
			{
				SetTime(timeSyncDummyEntity.GetSharedData(GamemodeCommon.Models.Data.DataKey.CurrentTime));
			}
		}

		public void OnPlayerVoiceStateChanged(bool enabled)
		{
			if (enabled) ShowVoice();
			else HideVoice();
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
			if (!Player.IsInVehicle()) return;

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
			Hours = Convert.ToInt32(data["hours"]);
			Minutes = Convert.ToInt32(data["minutes"]);
			Day = Convert.ToInt32(data["day"]);
			Month = Convert.ToInt32(data["month"]);
			UpdateTime(Hours, Minutes, Day, Month);
		}
	}
}
