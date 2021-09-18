
using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public static class NotificationType
	{
		public const string Success = "accept";
		public const string Error = "error";
	}

	public class Notification
	{
		[JsonProperty("text")]
		public string Text;

		[JsonProperty("delay")]
		public int Delay;

		[JsonProperty("closeTimeMs")]
		public int CloseTimeMs;

		[JsonProperty("type")]
		public string NotificationType;

		public Notification(string text, int delay, int closeTimeMs, string notificationType)
		{
			this.Text = text;
			this.Delay = delay;
			this.CloseTimeMs = closeTimeMs;
			this.NotificationType = notificationType;
		}
	}
}
