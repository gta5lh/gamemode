using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class GangWarStatistics
    {
        public GangWarStatistics(int fractionID)
        {
            this.FractionID = fractionID;

        }

        [JsonProperty("fraction_id")]
        public int FractionID { get; set; }

        [JsonProperty("kills_number")]
        public short KillsNumber { get; set; }
    }
}
