using System;
using GamemodeClient.Models;
using Newtonsoft.Json;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void InitGangWar(double remainingMs)
		{
			IndexCef.ExecuteJs($"InitGangWar({remainingMs})");
		}

		public static void StartGangWar(StartGangWar startGangWar)
		{
			string startGangWarJson = JsonConvert.SerializeObject(startGangWar);
			IndexCef.ExecuteJs($"StartGangWar('{startGangWarJson}')");
		}

		public static void UpdateStats(Stats stats)
		{
			string statsJson = JsonConvert.SerializeObject(stats);
			IndexCef.ExecuteJs($"UpdateStats('{statsJson}')");
		}

		public static void HideCapt()
		{
			IndexCef.ExecuteJs("HideCapt()");
		}
	}
}
