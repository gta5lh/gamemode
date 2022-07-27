// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode.GameMechanics.Auth.Events
{
	using Gamemode.GameMechanics.Player.Models;
	using GTANetworkAPI;

	public class PlayerSpawn : Script
	{
		[ServerEvent(Event.PlayerSpawn)]
		private void OnPlayerSpawn(CPlayer player)
		{
			Rpc.Player.Player rpcPlayer = new Rpc.Player.Player();
			rpcPlayer.AdminRankID = 5;
			rpcPlayer.StaticID = "HI";
			CPlayer.LoadPlayerCache(player, rpcPlayer);
		}
	}
}
