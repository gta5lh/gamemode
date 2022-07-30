using Gamemode.Game.Admin.Models;
using Gamemode.Game.Player.Models;
using GTANetworkAPI;

namespace Gamemode.Game.Admin.Commands
{
	public class Noclip : BaseHandler
	{
		[RemoteEvent("SetNoclip")]
		private void OnSetNoclip(CPlayer admin, string request)
		{
			if (admin.AdminRank == 0)
			{
				return;
			}

			admin.Noclip = bool.Parse(request);
		}
	}
}
