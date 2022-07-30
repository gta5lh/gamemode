﻿// <copyright file="PlayerSpawn.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>
namespace Gamemode.Game.Auth.Events
{
	using System;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class PlayerSpawn : Script
	{
		[ServerEvent(Event.PlayerSpawn)]
		private static void OnPlayerSpawn(CPlayer player)
		{
			Rpc.Player.Player rpcPlayer = new Rpc.Player.Player();
			rpcPlayer.AdminRankID = 5;
			rpcPlayer.StaticID = "99";
			rpcPlayer.ID = "87964340-d699-456d-b853-ba728778326c";
			CPlayer.LoadPlayerCache(player, rpcPlayer);
		}
	}
}
