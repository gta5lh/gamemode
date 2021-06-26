using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class GangWarStats
	{
		public short Ballas { get; set; }
		public short Bloods { get; set; }
		public short Marabunta { get; set; }
		public short Families { get; set; }
		public short Vagos { get; set; }

		GangWarStats()
		{

		}

		public GangWarStats(short ballas, short bloods, short marabunta, short families, short vagos)
		{
			this.Ballas = ballas;
			this.Bloods = bloods;
			this.Marabunta = marabunta;
			this.Families = families;
			this.Vagos = vagos;
		}
	}
}
