namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void LoginFailed(string errors)
		{
			IndexCef.ExecuteJs($"LoginFailed('{errors}')");
		}

		public static void ShowAuth()
		{
			Ui.OpenUI(IndexCef);
			IndexCef.ExecuteJs("ShowAuth()");
		}

		public static void HideAuth()
		{
			IndexCef.ExecuteJs("HideAuth()");
			Ui.CloseUI(IndexCef);
		}
	}
}
