using RAGE.Game;
using RAGE.Ui;
using System;

namespace GamemodeClient.Utils
{
	class DummyEntityUtil
	{
		public static RAGE.Elements.DummyEntity? GetByTypeID(int typeId)
		{
			if (RAGE.Elements.Entities.DummyEntities.Count <= 0) return null;

			foreach (RAGE.Elements.DummyEntity dummyEntity in RAGE.Elements.Entities.DummyEntities.All)
			{
				if (dummyEntity.DummyType == typeId) return dummyEntity;
			}

			return null;
		}
	}
}
