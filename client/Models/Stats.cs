using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class Stats
	{
		[JsonProperty("ballas")]
		public long Ballas;

		[JsonProperty("bloods")]
		public long Bloods;

		[JsonProperty("marabunta")]
		public long Marabunta;

		[JsonProperty("families")]
		public long Families;
		[JsonProperty("vagos")]
		public long Vagos;

		public Stats(long ballas, long bloods, long marabunta, long families, long vagos)
		{
			this.Ballas = ballas;
			this.Bloods = bloods;
			this.Marabunta = marabunta;
			this.Families = families;
			this.Vagos = vagos;
		}
	}
}
