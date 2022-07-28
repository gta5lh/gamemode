using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Player.Models;
using GTANetworkAPI;

namespace Gamemode.Mechanics.Admin.Commands
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

		private const string InvisibilityUsage = "Использование: /invisibility. Пример: /i";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("invisibility", InvisibilityUsage, Alias = "i", GreedyArg = true, Hide = true)]
		public void OnInvisibility(CPlayer admin)
		{
			if (admin.Noclip || admin.Spectating)
			{
				admin.SendChatMessage("Изменить невидимку нельзя с включенным noclip или spectate");
				return;
			}

			admin.Invisible = !admin.Invisible;
			NAPI.ClientEvent.TriggerClientEvent(admin, "SetInvisibility");
		}

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
