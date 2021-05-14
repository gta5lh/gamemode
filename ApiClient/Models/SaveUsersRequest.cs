using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SaveUsersRequest
    {
        public SaveUsersRequest(List<SaveUserRequest> saveUserRequests)
        {
            this.saveUserRequests = saveUserRequests;
        }

        [JsonProperty("users")]
        public List<SaveUserRequest> saveUserRequests { get; set; }
    }
}
