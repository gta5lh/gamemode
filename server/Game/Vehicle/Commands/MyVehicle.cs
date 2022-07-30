// <copyright file="MyVehicle.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vehicle.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.Vehicle.Models;
	using GamemodeCommon.Models;
	using Grpc.Core;
	using GTANetworkAPI;
	using Rpc.Errors;
	using Rpc.Player;

	public class MyVehicle : Script
	{
		private const string MyVehicleUsage = "Использование: /myvehicle. Пример: /myveh";

		[Command("myvehicle", MyVehicleUsage, Alias = "myveh", GreedyArg = true)]
		public static async Task OnMyVehicleAsync(CPlayer player)
		{
			MyVehicleRequest request = new MyVehicleRequest();
			request.PlayerID = player.PKId.ToString();

			MyVehicleResponse response;

			try
			{
				response = await Infrastructure.RpcClients.PlayerService.MyVehicleAsync(request);
			}
			catch (RpcException ex)
			{
				if (Error.IsEqualErrorCode(ex.StatusCode, ErrorCode.PlayerNotFound))
				{
					NAPI.Task.Run(() => player.SendChatMessage("У вас отсуствует автомобиль!"));
				}

				return;
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => player.SendChatMessage("Что-то пошло не так, попробуйте еще раз."));
				return;
			}

			NAPI.Task.Run(() =>
			{
				if (player.OneTimeVehicleId != null)
				{
					VehicleUtil.GetById(player.OneTimeVehicleId.Value).Delete();
				}

				CVehicle vehicle = (CVehicle)NAPI.Vehicle.CreateVehicle((uint)response.VehicleHash, player.Position, player.Heading, 0, 0, player.Name);
				vehicle.OwnerPlayerId = player.Id;
				vehicle.CustomPrimaryColor = new Color(0, 174, 255);
				vehicle.CustomSecondaryColor = new Color(0, 174, 255);
				vehicle.Rotation = new Vector3(0, 0, player.Heading);
				player.SetIntoVehicle(vehicle, 0);
				player.OneTimeVehicleId = vehicle.Id;

				player.SendNotification("Автомобиль успешно доставлен!", 0, 2000, NotificationType.Success);
			});
		}
	}
}
