using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SaveUserRequest
    {
        [JsonProperty("experience")]
        public short Experience { get; set; }

        public SaveUserRequest(short experience)
        {
            this.Experience = experience;
        }
    }
}
