// <copyright file="ExperienceController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeClient.Models;
	using Newtonsoft.Json;
	using RAGE;
	using static GamemodeClient.Controllers.Cef.Cef;

	public partial class HudController : Events.Script
	{
		private long previousExperience = 0;
		private long currentExperience = 0;
		private long requiredExperience = 0;

		private void OnRankedUp(object[] args)
		{
			RAGE.Game.Audio.PlayMissionCompleteAudio("FRANKLIN_BIG_01");
		}

		private void OnRankedDown(object[] args)
		{
			RAGE.Game.Audio.PlayMissionCompleteAudio("GENERIC_FAILED");
		}

		private void OnExperienceChanged(object[] args)
		{
			string updateExperienceJson = JsonConvert.SerializeObject(GamemodeClient.Utils.Minimap.GetMinimapAnchor());
			RAGE.Ui.Console.LogLine(RAGE.Ui.ConsoleVerbosity.Info, updateExperienceJson);
			this.previousExperience = (long)args[0];
			this.currentExperience = (long)args[1];
			this.requiredExperience = (long)args[2];

			UpdateExperience(new UpdateExperience(this.currentExperience, this.previousExperience, this.requiredExperience));
		}

		private void SetCurrentExperience()
		{
			UpdateExperience(new UpdateExperience(this.currentExperience, this.previousExperience, this.requiredExperience));
		}
	}
}
