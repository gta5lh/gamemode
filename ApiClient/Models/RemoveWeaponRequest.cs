using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class RemoveWeaponRequest
    {
        [JsonProperty("weapon_hash")]
        public WeaponHash WeaponHash { get; set; }

        [JsonProperty("removed_by")]
        public long RemovedBy { get; set; }

        public RemoveWeaponRequest(WeaponHash weaponHash, long removedBy)
        {
            WeaponHash = weaponHash;
            RemovedBy = removedBy;
        }
    }
}
