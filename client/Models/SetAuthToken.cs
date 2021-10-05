
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class SetAuthToken
	{
		[JsonProperty("email")]
		public string Email;

		[JsonProperty("token")]
		public string Token;

		public SetAuthToken(string email, string token)
		{
			this.Email = email;
			this.Token = token;
		}
	}
}
