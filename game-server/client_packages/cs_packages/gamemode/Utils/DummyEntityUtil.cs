// <copyright file="DummyEntityUtil.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Utils
{
	using System;
	using RAGE.Game;
	using RAGE.Ui;

	internal static class DummyEntityUtil
	{
		public static RAGE.Elements.DummyEntity? GetByTypeID(int typeId)
		{
			if (RAGE.Elements.Entities.DummyEntities.Count <= 0)
			{
				return null;
			}

			foreach (RAGE.Elements.DummyEntity dummyEntity in RAGE.Elements.Entities.DummyEntities.All)
			{
				if (dummyEntity.DummyType == typeId)
				{
					return dummyEntity;
				}
			}

			return null;
		}
	}
}
