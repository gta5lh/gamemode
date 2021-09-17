using GamemodeClient.Models;
using RAGE;
using RAGE.Ui;
using System.Collections.Generic;
using static RAGE.Events;
using static GamemodeClient.Controllers.Cef.Cef;
using GamemodeCommon.Models;

namespace GamemodeClient.Controllers
{
	public class NpcController : Events.Script
	{
		private static Dictionary<string, Models.Npc> Npcs = new Dictionary<string, Npc>
		{
			{ NpcNames.Bloods, new Bloods() },
			{ NpcNames.Marabunta, new Marabunta() },
			{ NpcNames.Vagos, new Vagos() },
			{ NpcNames.Ballas, new Ballas() },
			{ NpcNames.TheFamilies, new TheFamilies() },
			{ NpcNames.Trevor, new SpawnNpc(SpawnNpc.NameTrevor) },
			{ NpcNames.Michael, new SpawnNpc(SpawnNpc.NameMichael) },
			{ NpcNames.Franklin, new SpawnNpc(SpawnNpc.NameFranklin) },
		};

		private bool canInteractWithMenu;
		private string npcName;

		public NpcController()
		{
			Events.Add("DisplayPressE", this.OnDisplayPressE);
			Events.Add("CloseNpcMenu", this.OnCloseNpcMenu);
			Events.Add("NpcActionSelected", this.OnNpcActionSelected);
			Events.OnPlayerDeath += this.OnPlayerDeath;
			Events.Add("CreateWaypoint", this.OnCreateWaypoint);
		}

		#region Common
		private void OnDisplayPressE(object[] args)
		{
			this.npcName = "";
			this.canInteractWithMenu = (bool)args[0];
			HelpPopUpController.Instance.SetEnabled(this.canInteractWithMenu);
			HelpPopUpController.InteractKeyPressed = this.OnInteractKeyPressed;
			HelpPopUpController.ExitKeyPressed = this.OnExitKeyPressed;

			if (args.Length >= 2)
			{
				this.npcName = (string)args[1];
			}
		}

		private async void OnInteractKeyPressed()
		{
			if (Cursor.Visible || !this.canInteractWithMenu)
			{
				return;
			}

			Dialogue dialogue = await (Npcs[this.npcName].OnInitDialogue());
			InitNpcDialogue(dialogue);
		}

		private void OnExitKeyPressed()
		{
			if (!this.canInteractWithMenu)
			{
				return;
			}

			CloseNpcDialogue();
		}

		private void OnPlayerDeath(RAGE.Elements.Player player, uint reason, RAGE.Elements.Player killer, CancelEventArgs cancel)
		{
			this.OnDisplayPressE(new object[] { false });
			CloseNpcDialogue();
		}

		private void OnCloseNpcMenu(object[] args)
		{
			this.OnDisplayPressE(new object[] { false });
			CloseNpcDialogue();
		}

		private void OnNpcActionSelected(object[] args)
		{
			int action = (int)args[0];

			Npcs[this.npcName].OnActionSelected(action);
		}
		#endregion

		private void OnCreateWaypoint(object[] args)
		{
			RAGE.Game.Ui.SetNewWaypoint((float)args[0], (float)args[1]);
			this.canInteractWithMenu = false;
		}
	}
}
