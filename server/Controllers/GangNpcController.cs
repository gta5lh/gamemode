using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.Controllers
{
	public class GangNpcController : Script
	{
		[RemoteProc("IsGangMember", true)]
		private async Task<System.Object> OnIsGangMember(CustomPlayer player)
		{
			return player.Fraction != null;
		}

		[RemoteEvent("JoinGang")]
		private async Task OnJoinGang(CustomPlayer player, string request)
		{
			await GangService.SetAsGangMember(player, player.StaticId, GangUtil.GangIdByName[request], 1, player.StaticId);

			NAPI.Task.Run(() =>
			{
				player.SendChatMessage("Добро пожаловать в наши ряды!");
			});
		}

		[RemoteEvent("LeaveGang")]
		private async Task OnLeaveGang(CustomPlayer player, string request)
		{
			await GangService.SetAsGangMember(player, player.StaticId, 0, 0, player.StaticId);

			NAPI.Task.Run(() =>
			{
				player.SendChatMessage("Давай удачи!");
			});
		}
	}
}
