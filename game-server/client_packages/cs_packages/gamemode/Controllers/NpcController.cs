using RAGE;
using RAGE.Ui;
using static RAGE.Events;

namespace GamemodeClient.Controllers
{
	public class NpcController : Events.Script
	{
		private bool canInteractWithMenu;
		private string npcName;
		private string state;

		private HtmlWindow? npcMenu;

		public NpcController()
		{
			// Common
			Events.Add("DisplayPressE", this.OnDisplayPressE);
			Events.Add("CloseNpcMenu", this.OnCloseNpcMenu);

			// Spawn NPC
			Events.Add("PlayerSelectedGang", this.OnPlayerSelectedGang);
			Events.Add("CreateWaypoint", this.OnCreateWaypoint);

			// Gang NPC
			Events.Add("PlayerSelectedGangNpcAction", this.OnPlayerSelectedGangNpcAction);

			Events.OnPlayerDeath += this.OnPlayerDeath;
		}

		#region Common
		private void OnDisplayPressE(object[] args)
		{
			this.state = "";
			this.npcName = "";

			this.canInteractWithMenu = (bool)args[0];
			HelpPopUpController.Instance.SetEnabled(this.canInteractWithMenu);
			HelpPopUpController.InteractKeyPressed = this.OnInteractKeyPressed;
			HelpPopUpController.ExitKeyPressed = this.OnExitKeyPressed;

			if (args.Length >= 2)
			{
				this.npcName = (string)args[1];
			}

			if (args.Length >= 3)
			{
				this.state = (string)args[2];
			}
		}

		private void OnInteractKeyPressed()
		{
			if (Cursor.Visible)
			{
				return;
			}

			this.npcMenu = Controllers.Menu.Open(this.canInteractWithMenu, this.npcMenu, this.NpcMenuPath(this.npcName, this.state));
		}

		private void OnExitKeyPressed()
		{
			Controllers.Menu.Close(ref this.npcMenu);
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.canInteractWithMenu = false;
			Controllers.Menu.Close(ref this.npcMenu);
		}

		private void OnCloseNpcMenu(object[] args)
		{
			Controllers.Menu.Close(ref this.npcMenu);
		}

		private string NpcMenuPath(string npcName, string state)
		{
			if (state == string.Empty)
			{
				return $"package://cs_packages/gamemode/Frontend/Npc/Menu/{char.ToUpper(npcName[0]) + npcName.Substring(1)}/index.html";
			}

			return $"package://cs_packages/gamemode/Frontend/Npc/Menu/{char.ToUpper(npcName[0]) + npcName.Substring(1)}/index_{state}.html";
		}
		#endregion

		#region Spawn
		private void OnPlayerSelectedGang(object[] args)
		{
			Events.CallRemote("PlayerSelectedGang", (string)args[0]);
		}

		private void OnCreateWaypoint(object[] args)
		{
			RAGE.Game.Ui.SetNewWaypoint((float)args[0], (float)args[1]);
		}
		#endregion

		#region Gang
		private void OnPlayerSelectedGangNpcAction(object[] args)
		{
			Events.CallRemote("PlayerSelectedGangNpcAction", (string)args[0]);
		}

		#endregion
	}
}
