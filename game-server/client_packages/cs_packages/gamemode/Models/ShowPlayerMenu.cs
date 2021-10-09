using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class ShowPlayerMenu
	{
		[JsonProperty("playerName")]
		public string PlayerName;

		[JsonProperty("balance")]
		public long Balance;

		[JsonProperty("rank")]
		public string Rank;

		[JsonProperty("fraction")]
		public string Fraction;

		[JsonProperty("experience")]
		public string Experience;

		[JsonProperty("status")]
		public string Status;

		public ShowPlayerMenu(string playerName, long balance, string rank, string fraction, string experience, string status)
		{
			this.PlayerName = playerName;
			this.Balance = balance;
			this.Rank = rank;
			this.Fraction = fraction;
			this.Experience = experience;
			this.Status = status;
		}
	}
}
