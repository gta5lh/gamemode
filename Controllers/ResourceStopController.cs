// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
namespace Gamemode
{
    using System.Threading.Tasks;
    using Gamemode.Services.Player;
    using GTANetworkAPI;

    public class ResourceStopController : Script
    {
        public ResourceStopController()
        {

        }


        [ServerEvent(Event.ResourceStop)]
        private async Task OnResourceStop()
        {
            await UserService.SaveAllUsers();
            await NAPI.Task.WaitForMainThread();
        }
    }
}
 