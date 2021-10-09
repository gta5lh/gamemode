using System;
using GamemodeClient.Models;
using Newtonsoft.Json;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void ShowPlayerMenu(ShowPlayerMenu showPlayerMenu)
		{
			string showPlayerMenuJson = JsonConvert.SerializeObject(showPlayerMenu);
			IndexCef.ExecuteJs($"ShowPlayerMenu('{showPlayerMenuJson}')");
		}

		public static void HidePlayerMenu()
		{
			IndexCef.ExecuteJs($"HidePlayerMenu()");
		}
	}
}
