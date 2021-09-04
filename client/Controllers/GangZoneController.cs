using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Game;

namespace GamemodeClient.Controllers
{
	public class GangZoneController : Events.Script
	{
		private List<GamemodeCommon.Models.Gang.Zone> Zones;

		private int? FindZoneIndexByCords(float x, float y)
		{
			if (this.Zones == null)
			{
				return null;
			}

			for (int i = 0; i < this.Zones.Count; i++)
			{
				if (this.Zones[i].X == x && this.Zones[i].Y == y)
				{
					return i;
				}
			}

			return null;
		}

		public GangZoneController()
		{
			Events.Add("RenderGangZones", OnRenderGangzones);
			Events.Add("CaptureStart", OnCaptureStart);
			Events.Add("CaptureFinish", OnCaptureFinish);

			Models.Natives.SetThisScripCanRemoveBlipsCreatedByAnyScript(true);
		}

		public void OnRenderGangzones(object[] args)
		{
			if (args[0] == null) return;

			Zones = ((JArray)args[0]).ToObject<List<GamemodeCommon.Models.Gang.Zone>>();

			int blipId;
			while ((blipId = RAGE.Game.Ui.GetNextBlipInfoId(5)) != 0)
			{
				Vector3 cords = RAGE.Game.Ui.GetBlipCoords(blipId);
				int? zoneIndex = this.FindZoneIndexByCords(cords.X, cords.Y);
				if (zoneIndex != null)
				{
					this.SetBlipColor(blipId, this.Zones[zoneIndex.Value].BlipColor);
					this.Zones[zoneIndex.Value].BlipId = blipId;
				}
				else
				{
					if (RAGE.Game.Ui.DoesBlipExist(blipId))
					{
						RAGE.Game.Ui.SetBlipSprite(blipId, 0);
						RAGE.Game.Ui.RemoveBlip(ref blipId);
					}
				}
			}

			for (int i = 0; i < Zones.Count; i++)
			{
				GamemodeCommon.Models.Gang.Zone zone = Zones[i];
				if (zone.BlipId != null)
				{
					continue;
				}

				Zones[i].BlipId = this.CreateGangBlip(zone.X, zone.Y, zone.BlipColor);
			}

			for (int i = 0; i < Zones.Count; i++)
			{
				if (Zones[i].IsWarInProgress)
				{
					Invoker.Invoke(RAGE.Game.Natives.SetBlipFlashes, Zones[i].BlipId, true);
					continue;
				}

				Invoker.Invoke(RAGE.Game.Natives.SetBlipFlashes, Zones[i].BlipId, false);
			}
		}

		private void SetBlipColor(int blipId, byte color)
		{
			if (color < 0)
			{
				Invoker.Invoke(RAGE.Game.Natives.SetBlipColour, blipId, color * -1);
				Invoker.Invoke(RAGE.Game.Natives.SetBlipFlashes, blipId, true);
			}
			else
			{
				Invoker.Invoke(RAGE.Game.Natives.SetBlipColour, blipId, color);
			}
		}

		private int CreateGangBlip(float x, float y, byte color)
		{
			int blipId = RAGE.Game.Ui.AddBlipForRadius(x, y, 0, 50);
			Invoker.Invoke(RAGE.Game.Natives.SetBlipSprite, blipId, 5);
			Invoker.Invoke(RAGE.Game.Natives.SetBlipAlpha, blipId, 100);
			this.SetBlipColor(blipId, color);
			Invoker.Invoke(RAGE.Game.Natives.SetBlipRotation, blipId, 0);
			return blipId;
		}

		public void OnCaptureStart(object[] args)
		{
			long zoneID = (long)args[0];

			for (int i = 0; i < Zones.Count; i++)
			{
				if (Zones[i].Id == zoneID)
				{
					Invoker.Invoke(RAGE.Game.Natives.SetBlipFlashes, Zones[i].BlipId, true);
					return;
				}
			}
		}

		public void OnCaptureFinish(object[] args)
		{
			long zoneID = (long)args[0];
			int color = (int)args[1];

			for (int i = 0; i < Zones.Count; i++)
			{
				if (Zones[i].Id == zoneID)
				{
					Invoker.Invoke(RAGE.Game.Natives.SetBlipFlashes, Zones[i].BlipId, false);
					Invoker.Invoke(RAGE.Game.Natives.SetBlipColour, Zones[i].BlipId, color);
					return;
				}
			}
		}
	}
}
