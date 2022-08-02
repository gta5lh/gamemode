// <copyright file="GangZoneMgr.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.GangZones
{
	using System.Collections.Generic;
	using Newtonsoft.Json.Linq;
	using RAGE;
	using RAGE.Game;

	public class GangZoneMgr : Events.Script
	{
		private List<GamemodeCommon.Models.Gang.Zone> zones;

		public GangZoneMgr()
		{
			Events.Add("RenderGangZones", this.OnRenderGangzones);
			Events.Add("CaptureStart", this.OnCaptureStart);
			Events.Add("CaptureFinish", this.OnCaptureFinish);

			Game.Natives.Natives.SetThisScripCanRemoveBlipsCreatedByAnyScript(true);
		}

		public void OnRenderGangzones(object[] args)
		{
			if (args[0] == null)
			{
				return;
			}

			var jArray = (JArray)args[0];
			var zs = jArray.ToObject<List<GamemodeCommon.Models.Gang.Zone>>();
			if (zs == null)
			{
				return;
			}

			this.zones = zs;

			int blipId;
			while ((blipId = Ui.GetNextBlipInfoId(5)) != 0)
			{
				Vector3 cords = Ui.GetBlipCoords(blipId);
				int? zoneIndex = this.FindZoneIndexByCords(cords.X, cords.Y);
				if (zoneIndex != null)
				{
					this.SetBlipColor(blipId, this.zones[zoneIndex.Value].BlipColor);
					this.zones[zoneIndex.Value].BlipId = blipId;
				}
				else if (Ui.DoesBlipExist(blipId))
				{
					Ui.SetBlipSprite(blipId, 0);
					Ui.RemoveBlip(ref blipId);
				}
			}

			for (int i = 0; i < this.zones.Count; i++)
			{
				GamemodeCommon.Models.Gang.Zone zone = this.zones[i];
				if (zone.BlipId != null)
				{
					continue;
				}

				this.zones[i].BlipId = this.CreateGangBlip(zone.X, zone.Y, zone.BlipColor);
			}

			for (int i = 0; i < this.zones.Count; i++)
			{
				if (this.zones[i].IsWarInProgress)
				{
					Invoker.Invoke(Natives.SetBlipFlashes, this.zones[i].BlipId, true);
					continue;
				}

				Invoker.Invoke(Natives.SetBlipFlashes, this.zones[i].BlipId, false);
			}
		}

		public void OnCaptureStart(object[] args)
		{
			long zoneID = (long)args[0];

			for (int i = 0; i < this.zones.Count; i++)
			{
				if (this.zones[i].Id == zoneID)
				{
					Invoker.Invoke(Natives.SetBlipFlashes, this.zones[i].BlipId, true);
					return;
				}
			}
		}

		public void OnCaptureFinish(object[] args)
		{
			long zoneID = (long)args[0];
			int color = (int)args[1];

			for (int i = 0; i < this.zones.Count; i++)
			{
				if (this.zones[i].Id == zoneID)
				{
					Invoker.Invoke(Natives.SetBlipFlashes, this.zones[i].BlipId, false);
					Invoker.Invoke(Natives.SetBlipColour, this.zones[i].BlipId, color);
					return;
				}
			}
		}

		private int? FindZoneIndexByCords(float x, float y)
		{
			if (this.zones == null)
			{
				return null;
			}

			for (int i = 0; i < this.zones.Count; i++)
			{
				if (this.zones[i].X == x && this.zones[i].Y == y)
				{
					return i;
				}
			}

			return null;
		}

		private void SetBlipColor(int blipId, byte color)
		{
			Invoker.Invoke(Natives.SetBlipColour, blipId, color);
		}

		private int CreateGangBlip(float x, float y, byte color)
		{
			int blipId = Ui.AddBlipForRadius(x, y, 0, 50);
			Invoker.Invoke(Natives.SetBlipSprite, blipId, 5);
			Invoker.Invoke(Natives.SetBlipAlpha, blipId, 100);
			this.SetBlipColor(blipId, color);
			Invoker.Invoke(Natives.SetBlipRotation, blipId, 0);
			return blipId;
		}
	}
}
