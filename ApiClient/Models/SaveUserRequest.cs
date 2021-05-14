using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SaveUserRequest
    {
        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("experience")]
        public short Experience { get; set; }

        [JsonProperty("weapons")]
        public ICollection<Weapon>? Weapons { get; set; }

        [JsonProperty("money")]
        public long Money { get; set; }

        public SaveUserRequest(short experience, ICollection<Weapon>? weapons, long money)
        {
            this.Experience = experience;
            this.Weapons = weapons;
            this.Money = money;
        }

        public SaveUserRequest(long userId, short experience, ICollection<Weapon>? weapons, long money)
        {
            this.UserId = userId;
            this.Experience = experience;
            this.Weapons = weapons;
            this.Money = money;
        }
    }
}
