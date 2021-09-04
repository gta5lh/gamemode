using System.Collections.Generic;
using GamemodeClient.Models;
using RAGE;
using RAGE.Ui;
using static RAGE.Events;

namespace GamemodeClient.Controllers
{
	public delegate void OnInteractKeyPressed();
	public delegate void OnExitKeyPressed();

	public class HelpPopUpController : Script
	{
		public static HelpPopUpController Instance = new HelpPopUpController();
		public static OnInteractKeyPressed InteractKeyPressed;
		public static OnExitKeyPressed ExitKeyPressed;

		bool displayPopUp;

		public HelpPopUpController()
		{
			if (Instance != null) return;

			Events.Tick += this.OnTick;

			RAGE.Input.Bind(VirtualKeys.E, false, this.OnInteractKeyPressed);
			RAGE.Input.Bind(VirtualKeys.Escape, false, this.OnExitKeyPressed);
		}

		private void OnTick(List<Events.TickNametagData> nametags)
		{
			if (!this.displayPopUp)
			{
				return;
			}

			Natives.DisplayHelpText("Нажмите ~INPUT_CONTEXT~ для взаимодействия");
		}

		private void OnInteractKeyPressed()
		{
			if (InteractKeyPressed == null)
			{
				return;
			}

			InteractKeyPressed();
		}

		private void OnExitKeyPressed()
		{
			if (ExitKeyPressed == null)
			{
				return;
			}

			ExitKeyPressed();
		}

		public void Enable()
		{
			this.displayPopUp = true;
		}

		public void Disable()
		{
			this.displayPopUp = false;
		}

		public void SetEnabled(bool enabled)
		{
			this.displayPopUp = enabled;
		}
	}
}
