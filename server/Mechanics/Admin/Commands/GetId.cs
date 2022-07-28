// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Mechanics.Admin.Models;
	using Gamemode.Mechanics.Player;
	using GTANetworkAPI;

	public class GetId : Script
	{
		private const string IdTypeStatic = "s";
		private const string IdTypeDynamic = "d";
		private const string GetIdUsage = "Использование: /getid {s-статичный или d-динамичный} {id или имя}. Примеры: [/getid s 100], [/getid s lbyte00]";

		[Command("getid", GetIdUsage, Alias = "gid", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task OnGetId(GTANetworkAPI.Player player, string? idType = null, string? idOrPlayerName = null)
		{
			if (idType == null || idOrPlayerName == null || (idType != "s" && idType != "d"))
			{
				player.SendChatMessage(GetIdUsage);
				return;
			}

			string? staticId = null;

			try
			{
				_ = long.Parse(idOrPlayerName);
			}
			catch (Exception)
			{
				try
				{
					Rpc.Player.StaticIDByNameResponse idByNameResponse = await Infrastructure.RpcClients.PlayerService.StaticIDByNameAsync(new Rpc.Player.StaticIDByNameRequest(idOrPlayerName));
					staticId = idByNameResponse.StaticID;
				}
				catch (Exception)
				{

				}
			}

			string? expectedId = null;

			if (idType == IdTypeStatic)
			{
				expectedId = staticId == null ? IdsCache.StaticIdByDynamic(idOrPlayerName) : staticId;
			}
			else
			{
				expectedId = staticId == null ? IdsCache.DynamicIdByStatic(idOrPlayerName).ToString() : IdsCache.DynamicIdByStatic(staticId).ToString();
			}

			NAPI.Task.Run(() =>
			{
				if (expectedId == null)
				{
					if (staticId == null)
					{
						player.SendChatMessage($"Пользователь с ID {idOrPlayerName} не найден");
					}
					else
					{
						player.SendChatMessage($"Пользователь с именем {idOrPlayerName} не найден");
					}

					return;
				}

				player.SendChatMessage($"ID  = {expectedId}");
			});
		}
	}
}
