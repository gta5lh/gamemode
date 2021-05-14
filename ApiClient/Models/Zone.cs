using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class Zone
    {
        [JsonProperty("id")]
        public short Id { get; set; }

        [JsonProperty("x")]
        public short X { get; set; }

        [JsonProperty("Y")]
        public short Y { get; set; }

        [JsonProperty("fraction_id")]
        public byte FractionId { get; set; }

        [JsonProperty("battleworthy")]
        public bool Battleworthy { get; set; }

        public byte BlipColor { get; set; }
    }
}
