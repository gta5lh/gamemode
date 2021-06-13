using System;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class ReportAnswer
    {
        public ReportAnswer(long reportId, long adminId, string answer)
        {
            ReportId = reportId;
            AdminId = adminId;
            Answer = answer;
        }

        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("report_id")]
        public long ReportId { get; set; }

        [JsonProperty("admin_id")]
        public long AdminId { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
