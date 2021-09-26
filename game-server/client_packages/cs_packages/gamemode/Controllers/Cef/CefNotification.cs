using GamemodeClient.Models;
using Newtonsoft.Json;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void DisplayNotification(Notification notification)
		{
			string notificationJson = JsonConvert.SerializeObject(notification);
			IndexCef.ExecuteJs($"DisplayNotification('{notificationJson}')");
		}

		public static void HideAllNotifications()
		{
			IndexCef.ExecuteJs($"HideAllNotifications()");
		}
	}
}
