using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class GangWarStats
	{
		public long Ballas { get; set; }
		public long Bloods { get; set; }
		public long Marabunta { get; set; }
		public long Families { get; set; }
		public long Vagos { get; set; }

		GangWarStats()
		{

		}

		public GangWarStats(long ballas, long bloods, long marabunta, long families, long vagos)
		{
			this.Ballas = ballas;
			this.Bloods = bloods;
			this.Marabunta = marabunta;
			this.Families = families;
			this.Vagos = vagos;
		}
	}
}
