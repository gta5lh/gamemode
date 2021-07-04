using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class LoginUserRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("password")]
		public string Password { get; set; }

		[JsonProperty("last_ip_address")]
		public string LastIPAddress { get; set; }

		[JsonProperty("social_club_id")]
		public string SocialClubID { get; set; }

		[JsonProperty("computer_serial_number")]
		public string ComputerSerialNumber { get; set; }

		[JsonProperty("game_launcher_id")]
		public GameTypes GameLauncherType { get; set; }

		public LoginUserRequest(string email, string password, string lastIPAddress, string socialClubID, string computerSerialNumber, GameTypes gameLauncherType)
		{
			this.Email = email;
			this.Password = password;
			this.LastIPAddress = lastIPAddress;
			this.SocialClubID = socialClubID;
			this.ComputerSerialNumber = computerSerialNumber;
			this.GameLauncherType = gameLauncherType;
		}
	}
}
