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

			CPlayer? targetPlayer = PlayerUtil.GetById(targetId);
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

			// TODO
			// SpectateController.StartSpectate(admin, targetPlayer);
			Cache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} started spectate {targetPlayer.Name}");
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

			// TODO
			// SpectateController.StopSpectate(admin);
		}
	}
}
