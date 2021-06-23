// <copyright file="Position.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Commands.Admin;
	using Gamemode.Models.Admin;
	using Gamemode.Models.Player;
	using GTANetworkAPI;

	public class PositionCommand : Script
	{
		[Command("position", "Usage: /position {player_id}", Alias = "pos", SensitiveInfo = true, GreedyArg = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Position(CustomPlayer admin, string playerID)
		{
			if (admin.IsInVehicle)
			{
				admin.SendChatMessage($"X: {admin.Vehicle.Position.X}, Y: {admin.Vehicle.Position.Y}, Z: {admin.Vehicle.Position.Z}, Heading: {admin.Vehicle.Heading}");
			}
			else
			{
				admin.SendChatMessage($"X: {admin.Position.X}, Y: {admin.Position.Y}, Z: {admin.Position.Z}, Heading: {admin.Heading}");
			}
		}

		[Command("cameraposition", "Usage: /cameraposition {player_id}", Alias = "cpos", SensitiveInfo = true, GreedyArg = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void CameraPosition(CustomPlayer admin, string playerID)
		{
			NAPI.ClientEvent.TriggerClientEvent(admin, "ShowCameraPosition");
		}
	}
}
