
using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class Notification
	{
		[JsonProperty("text")]
		public string Text;

		[JsonProperty("delay")]
		public long Delay;

		[JsonProperty("closeTimeMs")]
		public long CloseTimeMs;

		[JsonProperty("type")]
		public string NotificationType;

		public Notification(string text, long delay, long closeTimeMs, string notificationType)
		{
			this.Text = text;
			this.Delay = delay;
			this.CloseTimeMs = closeTimeMs;
			this.NotificationType = notificationType;
		}
	}
}
