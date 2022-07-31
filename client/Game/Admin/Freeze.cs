// <copyright file="Freeze.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeCommon.Models.Data;
	using RAGE;
	using RAGE.Elements;

	public class Freeze : Events.Script
	{
		static Freeze()
		{
			Events.AddDataHandler(DataKey.IsFreezed, OnIsFreezed);
			Events.OnEntityStreamIn += OnEntityStreamIn;
		}

		private static void OnIsFreezed(Entity entity, object arg, object oldArg)
		{
			if (entity.Type != Type.Player)
			{
				return;
			}

			bool isFreezed = (bool)arg;

			if (entity.Id == Game.Player.Models.Player.CurrentPlayer.Id)
			{
				Game.Player.Models.Player.CurrentPlayer.FreezePosition(isFreezed);
			}
			else
			{
				if (Entities.Players == null || Entities.Players.Count == 0)
				{
					return;
				}

				RAGE.Elements.Player target = (RAGE.Elements.Player)entity;

				foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
				{
					if (!player.Exists || player.Id != target.Id)
					{
						continue;
					}

					player.FreezePosition(isFreezed);
					return;
				}
			}
		}

		private static void OnEntityStreamIn(RAGE.Elements.Entity entity)
		{
			if (entity.Type != Type.Player)
			{
				return;
			}

			if (entity.GetSharedData(DataKey.IsFreezed) == null)
			{
				return;
			}

			bool isFreezed = (bool)entity.GetSharedData(DataKey.IsFreezed);
			((RAGE.Elements.Player)entity).FreezePosition(isFreezed);
		}
	}
}
