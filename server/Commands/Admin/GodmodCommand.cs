using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
	public class GodmodCommand : BaseCommandHandler
	{
		private const string GodmodCommandUsage = "Использование: /godmod. Пример: /gm";

		[Command("godmod", GodmodCommandUsage, Alias = "gm", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Godmod(CustomPlayer admin)
		{
			if (admin.Noclip || admin.Spectating)
			{
				admin.SendChatMessage("Изменить godmod нельзя с включенным noclip или spectate");
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(admin, "SetGodmod");
		}

		private const string InvisibilityCommandUsage = "Использование: /invisibility. Пример: /i";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("invisibility", InvisibilityCommandUsage, Alias = "i", GreedyArg = true, Hide = true)]
		public void Invisibility(CustomPlayer admin)
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
		private void OnSetNoclip(CustomPlayer admin, string request)
		{
			if (admin.AdminRank == 0)
			{
				return;
			}

			admin.Noclip = bool.Parse(request);
		}
	}
}
