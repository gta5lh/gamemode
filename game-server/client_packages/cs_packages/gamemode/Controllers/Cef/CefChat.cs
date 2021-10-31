using System;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void OpenChat()
		{
			IndexCef.ExecuteJs($"OpenChat()");
		}

		public static void CloseChat()
		{
			IndexCef.ExecuteJs($"CloseChat()");
		}
	}
}
