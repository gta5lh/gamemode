using GamemodeClient.Models;
using Newtonsoft.Json;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void ShowCarPark(ShowCarPark showCarPark)
		{
			string showCarParkJson = JsonConvert.SerializeObject(showCarPark);
			IndexCef.ExecuteJs($"ShowCarPark('{showCarParkJson}')");
			Controllers.Menu.Open(true);
			disableUIStateChangedEvent(false);
		}

		public static void CloseCarPark()
		{
			IndexCef.ExecuteJs($"CloseCarPark()");
			Controllers.Menu.Close();
			disableUIStateChangedEvent(true);
		}
	}
}
