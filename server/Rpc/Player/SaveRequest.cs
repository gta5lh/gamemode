// <copyright file="SaveRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;
	using System.Collections.Generic;

	public partial class SaveRequest
	{
		public SaveRequest(Guid id, long experience, long money, List<Weapon> weapons, long health, long armor)
		{
			this.ID = id.ToString();
			this.Experience = experience;
			this.Money = money;
			this.Weapons.Add(weapons);
			this.Health = health;
			this.Armor = armor;
		}
	}
}
