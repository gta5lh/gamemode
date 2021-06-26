using System.Collections.Generic;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class FinishGangWarRequest
    {
        public FinishGangWarRequest(bool? failed, byte? winnerFractionID, ICollection<GangWarStatistics>? gangWarStatistics)
        {
            this.Failed = failed;
            this.WinnerFractionID = winnerFractionID;
            this.GangWarStatistics = gangWarStatistics;
        }

        public FinishGangWarRequest(bool? failed)
        {
            this.Failed = failed;
        }

        [JsonProperty("failed")]
        public bool? Failed { get; set; }

        [JsonProperty("winner_fraction_id")]
        public byte? WinnerFractionID { get; set; }

        [JsonProperty("gang_war_statistics")]
        public ICollection<GangWarStatistics>? GangWarStatistics { get; set; }
    }
}
