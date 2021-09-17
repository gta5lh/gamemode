
using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class UpdateExperience
	{
		[JsonProperty("current")]
		public long Current;

		[JsonProperty("prevCurrent")]
		public long PrevCurrent;

		[JsonProperty("total")]
		public long Total;

		public UpdateExperience(long current, long prevCurrent, long total)
		{
			this.Current = current;
			this.PrevCurrent = prevCurrent;
			this.Total = total;
		}
	}
}
