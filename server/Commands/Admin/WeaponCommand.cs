// <copyright file="Weapon.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Commands.Admin;
	using Gamemode.Models.Player;
	using GTANetworkAPI;
	using Rpc.User;
	using Gamemode.Cache.Player;

	public class WeaponCommand : BaseCommandHandler
	{
		private const string WeaponCommandUsage = "Usage: /weapon {static_id} {название} {кол-во_патрон}. Пример: /weapon 0 pistol 100";

		[Command("weapon", WeaponCommandUsage, Alias = "w", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Senior)]
		public async Task Weapon(CustomPlayer admin, string targetStaticIdInput = null, string weaponName = null, string amountInput = null)
		{
			if (targetStaticIdInput == null || weaponName == null || amountInput == null)
			{
				admin.SendChatMessage(WeaponCommandUsage);
				return;
			}

			int amount = 0;
			long targetStaticId = 0;

			try
			{
				amount = int.Parse(amountInput);
				targetStaticId = long.Parse(targetStaticIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(WeaponCommandUsage);
				return;
			}

			WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(weaponName);
			if (weaponHash == 0)
			{
				admin.SendChatMessage($"Неизвестное название оружия: {weaponName}");
				return;
			}

			string targetName = "";
			CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);

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
					GiveWeaponResponse giveWeaponResponse = await Infrastructure.RpcClients.UserService.GiveWeaponAsync(new GiveWeaponRequest(targetStaticId, weaponHash, amount, admin.StaticId));
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
				AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} выдал оружие {targetName}. Название: {weaponName}. Кол-во патрон: {amount}");
				this.Logger.Warn($"Administrator {admin.Name} gave weapon to {targetName}. Name: {weaponName}. Amount: {amount}");
			});
		}

		private const string RemoveWeaponCommandUsage = "Usage: /removeweapon {static_id} {название}. Пример: /weapon 0 pistol";

		[Command("removeweapon", RemoveWeaponCommandUsage, Alias = "rw", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Senior)]
		public async Task TakeWeapon(CustomPlayer admin, string targetStaticIdInput = null, string weaponName = null)
		{
			if (targetStaticIdInput == null || weaponName == null)
			{
				admin.SendChatMessage(RemoveWeaponCommandUsage);
				return;
			}

			long targetStaticId = 0;

			try
			{
				targetStaticId = long.Parse(targetStaticIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(WeaponCommandUsage);
				return;
			}

			WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(weaponName);
			if (weaponHash == 0)
			{
				admin.SendChatMessage($"Неизвестное название оружия: {weaponName}");
				return;
			}

			string targetName = "";
			CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);

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
					RemoveWeaponResponse removeWeaponResponse = await Infrastructure.RpcClients.UserService.RemoveWeaponAsync(new RemoveWeaponRequest(targetStaticId, weaponHash, admin.StaticId));
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
				AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} забрал оружие у {targetName}. Название: {weaponName}");
				this.Logger.Warn($"Administrator {admin.Name} removed weapon from {targetName}. Name: {weaponName}");
			});
		}
	}
}
