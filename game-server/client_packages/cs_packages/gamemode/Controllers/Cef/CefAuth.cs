using GamemodeClient.Models;
using Newtonsoft.Json;
using RAGE;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void ResetPasswordSucceed()
		{
			IndexCef.ExecuteJs($"ResetPasswordSucceed()");
		}

		public static void ResetPasswordFailed(string errors)
		{
			IndexCef.ExecuteJs($"ResetPasswordFailed('{errors}')");
		}

		public static void RegisterFailed(string errors)
		{
			IndexCef.ExecuteJs($"RegisterFailed('{errors}')");
		}

		public static void LoginFailed(string errors)
		{
			IndexCef.ExecuteJs($"LoginFailed('{errors}')");
		}

		public static void ShowAuth()
		{
			Ui.OpenUI();
			IndexCef.ExecuteJs("ShowAuth()");
		}

		public static void HideAuth()
		{
			IndexCef.ExecuteJs("HideAuth()");
			Ui.CloseUI();
		}

		public static void SetAuthToken(SetAuthToken setAuthToken)
		{
			string json = JsonConvert.SerializeObject(setAuthToken);
			RAGE.Ui.Console.Log(RAGE.Ui.ConsoleVerbosity.Error, json);
			IndexCef.ExecuteJs($"SetAuthToken('{json}')");
		}
	}
}
