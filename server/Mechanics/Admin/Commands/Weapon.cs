﻿// <copyright file="Weapon.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Mechanics.Admin.Commands;
	using GTANetworkAPI;
	using Gamemode.Mechanics.Admin.Models;
	using Gamemode.Mechanics.Player.Models;
	using Rpc.Player;
	using Gamemode.Mechanics.Admin;

	public class Weapon : BaseHandler
	{
		private const string WeaponUsage = "Usage: /weapon {static_id} {название} {кол-во_патрон}. Пример: /weapon 0 pistol 100";

		[Command("weapon", WeaponUsage, Alias = "w", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Senior)]
		public async Task OnWeapon(CPlayer admin, string? targetStaticIdInput = null, string? weaponName = null, string? amountInput = null)
		{
			if (targetStaticIdInput == null || weaponName == null || amountInput == null)
			{
				admin.SendChatMessage(WeaponUsage);
				return;
			}

			int amount = 0;
			string targetStaticId;

			try
			{
				amount = int.Parse(amountInput);
				targetStaticId = targetStaticIdInput;
			}
			catch (Exception)
			{
				admin.SendChatMessage(WeaponUsage);
				return;
			}

			WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(weaponName);
			if (weaponHash == 0)
			{
				admin.SendChatMessage($"Неизвестное название оружия: {weaponName}");
				return;
			}

			string targetName = "";
			CPlayer? targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);

			if (targetPlayer != null)
			{
				targetName = targetPlayer.Name;
				targetPlayer.CustomGiveWeapon(weaponHash, amount);

				if (targetPlayer.StaticId != admin.StaticId)
				{
					targetPlayer.SendChatMessage($"Администратор {admin.Name} [{admin.StaticId}] выдал вам оружие. Название: {weaponName}. Кол-во патрон: {amount}");
				}
			}
			else
			{
				try
				{
					GiveWeaponResponse giveWeaponResponse = await Infrastructure.RpcClients.PlayerService.GiveWeaponAsync(new GiveWeaponRequest(targetStaticId, weaponHash, amount, admin.StaticId));
					targetName = giveWeaponResponse.Name;
				}
				catch (Exception)
				{
					NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetStaticId} не найден"));
					return;
				}
			}

			NAPI.Task.Run(() =>
			{
				Cache.SendMessageToAllAdminsAction($"{admin.Name} выдал оружие {targetName}. Название: {weaponName}. Кол-во патрон: {amount}");
				this.Logger.Warn($"Administrator {admin.Name} gave weapon to {targetName}. Name: {weaponName}. Amount: {amount}");
			});
		}

		private const string RemoveWeaponUsage = "Usage: /removeweapon {static_id} {название}. Пример: /weapon 0 pistol";

		[Command("removeweapon", RemoveWeaponUsage, Alias = "rw", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Senior)]
		public async Task OnTakeWeapon(CPlayer admin, string? targetStaticIdInput = null, string? weaponName = null)
		{
			if (targetStaticIdInput == null || weaponName == null)
			{
				admin.SendChatMessage(RemoveWeaponUsage);
				return;
			}

			string targetStaticId;

			try
			{
				targetStaticId = targetStaticIdInput;
			}
			catch (Exception)
			{
				admin.SendChatMessage(WeaponUsage);
				return;
			}

			WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(weaponName);
			if (weaponHash == 0)
			{
				admin.SendChatMessage($"Неизвестное название оружия: {weaponName}");
				return;
			}

			string targetName = "";
			CPlayer? targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);

			if (targetPlayer != null)
			{
				targetName = targetPlayer.Name;
				targetPlayer.CustomRemoveWeapon(weaponHash);

				if (targetPlayer.StaticId != admin.StaticId)
				{
					targetPlayer.SendChatMessage($"Администратор {admin.Name} [{admin.StaticId}] забрал ваше оружие. Название: {weaponName}");
				}
			}
			else
			{
				try
				{
					RemoveWeaponResponse removeWeaponResponse = await Infrastructure.RpcClients.PlayerService.RemoveWeaponAsync(new RemoveWeaponRequest(targetStaticId, weaponHash, admin.StaticId));
					targetName = removeWeaponResponse.Name;
				}
				catch (Exception)
				{
					NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetStaticId} не найден"));
					return;
				}
			}

			NAPI.Task.Run(() =>
			{
				Cache.SendMessageToAllAdminsAction($"{admin.Name} забрал оружие у {targetName}. Название: {weaponName}");
				this.Logger.Warn($"Administrator {admin.Name} removed weapon from {targetName}. Name: {weaponName}");
			});
		}
	}
}
