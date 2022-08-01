// <copyright file="LogoutRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using System.Collections.Generic;

	public partial class LogoutRequest
	{
		public LogoutRequest(Guid id, long money, long experience, List<Weapon> weapons, long health, long armor)
		{
			this.ID = id.ToString();
			this.Money = money;
			this.Experience = experience;
			this.Weapons.Add(weapons);
			this.Health = health;
			this.Armor = armor;
		}
	}
}
