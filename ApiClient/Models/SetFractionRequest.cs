using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SetFractionRequest
    {
        [JsonProperty("fraction")]
        public short Fraction { get; set; }

        [JsonProperty("tier")]
        public short Tier { get; set; }

        [JsonProperty("set_by")]
        public long SetBy { get; set; }

        public SetFractionRequest(short fraction, short tier, long setBy)
        {
            this.Fraction = fraction;
            this.Tier = tier;
            this.SetBy = setBy;
        }
    }
}
