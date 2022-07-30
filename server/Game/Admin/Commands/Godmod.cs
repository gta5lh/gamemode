using Gamemode.Game.Admin.Models;
using Gamemode.Game.Player.Models;
using GTANetworkAPI;

namespace Gamemode.Game.Admin.Commands
{
	public class Godmod : BaseHandler
	{
		private const string GodmodUsage = "Использование: /godmod. Пример: /gm";

		[Command("godmod", GodmodUsage, Alias = "gm", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnGodmod(CPlayer admin)
		{
			if (admin.Noclip || admin.Spectating)
			{
				admin.SendChatMessage("Изменить godmod нельзя с включенным noclip или spectate");
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(admin, "SetGodmod");
		}
	}
}
