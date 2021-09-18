using GamemodeClient.Models;
using Newtonsoft.Json;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void ShowWeaponShop(ShowWeaponShop showWeaponShop)
		{
			string showWeaponShopJson = JsonConvert.SerializeObject(showWeaponShop);
			IndexCef.ExecuteJs($"ShowWeaponShop('{showWeaponShopJson}')");
			Controllers.Menu.Open(true);
			disableUIStateChangedEvent(false);
		}

		public static void CloseWeaponShop()
		{
			IndexCef.ExecuteJs($"CloseWeaponShop()");
			Controllers.Menu.Close();
			disableUIStateChangedEvent(true);
		}
	}
}
