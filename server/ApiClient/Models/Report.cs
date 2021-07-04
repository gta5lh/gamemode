using System;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class Report
	{
		public Report(long userId, string question)
		{
			UserId = userId;
			Question = question;
		}

		[JsonProperty("id")]
		public long? Id { get; set; }

		[JsonProperty("user_id")]
		public long UserId { get; set; }

		[JsonProperty("question")]
		public string Question { get; set; }

		[JsonProperty("status")]
		public string? Status { get; set; }

		[JsonProperty("created_at")]
		public DateTime? CreatedAt { get; set; }

	}
}
