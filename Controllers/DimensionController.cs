using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
    public class DimensionController : Script
    {
        [RemoteProc("SetOwnDimension")]
        private uint OnSetOwnDimension(CustomPlayer player)
        {
            player.Dimension = player.Id + (uint)1;
            return player.Dimension;
        }

        [RemoteEvent("SetServerDimension")]
        private void OnSetServerDimension(CustomPlayer player)
        {
            player.Dimension = 0;
        }
    }
}
