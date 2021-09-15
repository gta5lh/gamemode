
namespace Gamemode.Models.Npc
{
	using GTANetworkAPI;
	using GamemodeCommon.Models;

	public static class SpawnNpcs
	{
		private static readonly Npc[] spawnNpcs = new Npc[]
		{
			new Npc(new Vector3(-1032.5, -2734.5, 20.17), 133f, "Тревор", PedHash.Trevor, new Colshape.SpawnNpcEvent(NpcNames.Trevor), 307, "Аэропорт"), // аэропорт
            new Npc(new Vector3(412.6, -633.9, 28.5), -178f, "Франклин", PedHash.Franklin, new Colshape.SpawnNpcEvent(NpcNames.Franklin), 513, "Автобусная станция"), // автобусная станция
            new Npc(new Vector3(-422.48, 1126, 325.9), 145f, "Майкл", PedHash.Michael, new Colshape.SpawnNpcEvent(NpcNames.Michael), 564, "Обсерватория"), // обсерватория
        };

		public static void CreateSpawnNpcs()
		{
			foreach (Npc npc in spawnNpcs)
			{
				npc.Create();
			}
		}
	}
}
