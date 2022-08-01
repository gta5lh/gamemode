// <copyright file="Spectate.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Spectate : BaseHandler
	{
		private const string SpectateUsage = "Использование: /spectate {player_id}. Пример: /spectate 10";
		private const string SpectateStopUsage = "Использование: /spectatestop. Пример: /specstop";

		[RemoteEvent("TrySpectate")]
		public static void TrySpectate(CPlayer admin, int id)
		{
			Player? found = NAPI.Pools.GetAllPlayers().Find(x => x.Id == id);
			if (found == null)
			{
				StopSpectate(admin);
				return;
			}

			StartSpectate(admin, (CPlayer)found);
		}

		public static void StartSpectate(CPlayer admin, CPlayer targetPlayer)
		{
			if (admin.SpectatePosition == null)
			{
				admin.SpectatePosition = admin.Position;
			}

			admin.Spectating = true;
			admin.RemoveAllWeapons();
			admin.Position = targetPlayer.Position + new Vector3(0, 0, 10);
			admin.Dimension = targetPlayer.Dimension;

			admin.TriggerEvent("spectate", targetPlayer.Id);
		}

		public static void StopSpectate(CPlayer admin)
		{
			admin.Position = admin.SpectatePosition;
			admin.Dimension = 0;
			admin.Spectating = false;
			admin.SpectatePosition = null;
			admin.TriggerEvent("spectateStop");
		}

		[AdminMiddleware(AdminRank.Junior)]
		[Command("spectatestop", SpectateStopUsage, Alias = "specstop", GreedyArg = true, Hide = true)]
		public static void OnSpectateStop(CPlayer admin)
		{
			if (admin.SpectatePosition == null)
			{
				admin.SendChatMessage("Вы не в режиме слежения");
				return;
			}

			StopSpectate(admin);
		}

		[Command("spectate", SpectateUsage, Alias = "spec", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnSpectate(CPlayer admin, string? targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				admin.SendChatMessage(SpectateUsage);
				return;
			}

			if (admin.Noclip)
			{
				admin.SendChatMessage("Нельзя начать слежение в режиме Noclip");
				return;
			}

			ushort targetId;
			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SpectateUsage);
				return;
			}

			CPlayer? targetPlayer = Gamemode.Game.Player.Util.GetById(targetId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
				return;
			}

			if (targetPlayer == admin)
			{
				admin.SendChatMessage("Нельзя начать слежение за самим собой");
				return;
			}

			StartSpectate(admin, targetPlayer);
			Cache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} started spectate {targetPlayer.Name}");
		}
	}
}
