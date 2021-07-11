using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using GamemodeCommon.Models.Data;

namespace GamemodeClient.Controllers
{
	public class PlayerBlipController : Events.Script
	{
		public PlayerBlipController()
		{
			Events.Tick += OnTick;
			Events.OnEntityStreamIn += OnEntityStreamIn;
			Events.OnEntityStreamOut += OnEntityStreamOut;
			Events.AddDataHandler(DataKey.BlipColor, OnBlipColorUpdate);
		}

		public void OnTick(List<Events.TickNametagData> nametags)
		{
			foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
			{
				if (player == RAGE.Elements.Player.LocalPlayer || !player.HasData("blip")) continue;

				Blip blip = player.GetData<Blip>("blip");

				blip.SetCoords(player.Position.X, player.Position.Y, player.Position.Z);
				blip.SetRotation((int)player.GetHeading());
			}
		}

		public void OnEntityStreamIn(RAGE.Elements.Entity entity)
		{
			if (entity.Type != Type.Player)
			{
				return;
			}

			object color = entity.GetSharedData(DataKey.BlipColor);
			if (color == null || (int)color == -1) return;

			CreatePlayerBlip((RAGE.Elements.Player)entity, (int)color);
		}

		public void OnEntityStreamOut(RAGE.Elements.Entity entity)
		{
			if (entity.Type != Type.Player)
			{
				return;
			}

			if (entity.HasData("blip")) entity.GetData<Blip>("blip").Destroy();
		}

		private void CreatePlayerBlip(RAGE.Elements.Player player, int color)
		{
			Blip newBlip = new Blip(1, player.Position, player.Name, 1, color, 255, 0, false, 0, 0, RAGE.Elements.Player.LocalPlayer.Dimension);
			newBlip.SetCategory(7);
			newBlip.ShowHeadingIndicatorOn(true);
			player.SetData("blip", newBlip);
		}

		public void OnBlipColorUpdate(RAGE.Elements.Entity entity, object arg, object oldArg)
		{
			RAGE.Elements.Player player = (RAGE.Elements.Player)entity;
			if (!player.Exists || player == Player.CurrentPlayer)
			{
				return;
			}

			int color = (int)arg;
			bool hasBlip = player.HasData("blip");

			if (color != -1 && hasBlip)
			{
				player.GetData<Blip>("blip").SetColour(color);
			}
			else if (color == -1 && hasBlip)
			{
				player.GetData<Blip>("blip").Destroy();
				player.ResetData("blip");
			}
			else if (color != -1 && !hasBlip)
			{
				CreatePlayerBlip(player, color);
			}
		}
	}
}
