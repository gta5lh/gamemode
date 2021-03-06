using System.Collections.Generic;

namespace Gamemode.Controllers
{
    using System.Threading.Tasks;
    using Gamemode.Api;
    using Gamemode.Models.Authentication;
    using Gamemode.Models.User;
    using GTANetworkAPI;
    using Newtonsoft.Json;

    public class AuthenticationController : Script
    {
        [RemoteEvent("LoginSubmitted")]
        private async Task OnLoginSubmitted(Player player, string request)
        {
            LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>(request);
            List<string> invalidFieldNames = loginRequest.Validate();
            if (invalidFieldNames.Count > 0)
            {
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            User user = await Client.GetUserByEmailAndPasswordAsync(loginRequest);
            if (user == null)
            {
                invalidFieldNames = new List<string>(new string[] { "email", "password" });
                NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
                return;
            }

            NAPI.Task.Run(() => player.SendChatMessage(user.Email));

            NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
        }

        [RemoteEvent("RegisterSubmitted")]
        private async Task OnRegisterSubmitted(Player player, string request)
        {
            LoginRequest registerRequest = JsonConvert.DeserializeObject<LoginRequest>(request);

            NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
        }
    }
}
