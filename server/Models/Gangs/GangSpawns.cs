using System.Collections.Generic;

namespace Gamemode.Models.Gangs
{
	using Gamemode.Models.Spawn;

	public class GangSpawns
	{
		public static readonly Dictionary<byte, Spawn> Spawns = new Dictionary<byte, Spawn>()
		{
			{ 1, Bloods.Spawn},
			{ 2, Ballas.Spawn},
			{ 3, TheFamilies.Spawn},
			{ 4, Vagos.Spawn},
			{ 5, Marabunta.Spawn}
		};

		public static readonly Dictionary<string, Spawn> SpawnByGangName = new Dictionary<string, Spawn>()
		{
			{ "bloods", Bloods.Spawn},
			{ "ballas", Ballas.Spawn},
			{ "the_families", TheFamilies.Spawn},
			{ "vagos", Vagos.Spawn},
			{ "marabunta", Marabunta.Spawn}
		};
	}
}
