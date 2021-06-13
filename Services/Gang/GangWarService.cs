using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services
{
    public static class GangWarService
    {
        public static async Task FinishGangWarAsFailed()
        {
            try
            {
                await ApiClient.ApiClient.FinishGangWar(new FinishGangWarRequest(true));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
