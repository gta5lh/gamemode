using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class ShowWeaponShop
	{
		[JsonProperty("money")]
		public long Money;

		public ShowWeaponShop(long money)
		{
			this.Money = money;
		}
	}
}
