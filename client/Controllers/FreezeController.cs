﻿// <copyright file="FreezeController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Elements;

	public class FreezeController : Events.Script
	{
		public FreezeController()
		{
			Events.AddDataHandler("is_freezed", this.OnIsFreezed);
			Events.OnEntityStreamIn += OnEntityStreamIn;
		}

		private void OnIsFreezed(Entity entity, object arg, object oldArg)
		{
			if (entity.Type != Type.Player) return;

			bool isFreezed = (bool)arg;

			if (entity.Id == Player.CurrentPlayer.Id) Player.CurrentPlayer.FreezePosition(isFreezed);
			else
			{
				if (Entities.Players == null || Entities.Players.Count == 0) return;

				RAGE.Elements.Player target = (RAGE.Elements.Player)entity;

				foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
				{
					if (!player.Exists || player.Id != target.Id) continue;

					player.FreezePosition(isFreezed);
					return;
				}
			}
		}

		public void OnEntityStreamIn(RAGE.Elements.Entity entity)
		{
			if (entity.Type != Type.Player) return;
			if (entity.GetSharedData("is_freezed") == null) return;

			bool isFreezed = (bool)entity.GetSharedData("is_freezed");
			((RAGE.Elements.Player)entity).FreezePosition(isFreezed);
		}
	}
}
