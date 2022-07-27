// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.GameMechanics.Admin.Models;
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

			long? staticId = null;

			try
			{
				_ = long.Parse(idOrPlayerName);
			}
			catch (Exception)
			{
				try
				{
					// TODO
					// IDByNameResponse idByNameResponse = await Infrastructure.RpcClients.PlayerService.IDByNameAsync(new IDByNameRequest(idOrPlayerName));
					// staticId = idByNameResponse.ID;
				}
				catch (Exception)
				{

				}
			}

			long? expectedId = null;

			if (idType == IdTypeStatic)
			{
				// TODO
				// expectedId = staticId == null ? IdsCache.StaticIdByDynamic(idOrPlayerName) : staticId;
			}
			else
			{
				// TODO
				// expectedId = staticId == null ? IdsCache.DynamicIdByStatic(idOrPlayerName) : IdsCache.DynamicIdByStatic(staticId.Value);
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
