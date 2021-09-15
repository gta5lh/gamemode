// <copyright file="ExperienceController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeClient.Models;
	using RAGE;

	public class ExperienceController : Events.Script
	{
		private const int StartExperience = 0;
		private const int RankBar = 19;
		private const int DecreaseColor = 6;
		private const int IncreaseColor = 1166;
		private const int Opasity = 100;

		public ExperienceController()
		{
			Events.Add("ExperienceChanged", this.OnExperienceChanged);
			Events.Add("RankedUp", this.OnRankedUp);
			Events.Add("RankedDown", this.OnRankedDown);
		}

		private async void OnRankedUp(object[] args)
		{
			RAGE.Game.Audio.PlayMissionCompleteAudio("FRANKLIN_BIG_01");
		}

		private async void OnRankedDown(object[] args)
		{
			RAGE.Game.Audio.PlayMissionCompleteAudio("GENERIC_FAILED");
		}

		private async void OnExperienceChanged(object[] args)
		{
			long previousExperience = (long)args[0];
			long currentExperience = (long)args[1];
			long requiredExperience = (long)args[2];
			long currentLevel = (long)args[3];

			this.DisplayRankBar(requiredExperience, previousExperience, currentExperience, currentLevel);
		}

		private bool rankBarHidden = false;

		private async void DisplayRankBar(long requiredExperience, long previousExperience, long currentExperience, long currentLevel)
		{
			if (this.rankBarHidden)
			{
				return;
			}

			if (GangWarController.Hide())
			{
				this.rankBarHidden = true;
			}

			int color = IncreaseColor;
			if (currentExperience < previousExperience)
			{
				color = DecreaseColor;
			}

			while (!Natives.HasScaleformScriptHudMovieLoaded(RankBar))
			{
				Natives.RequestScaleformScriptHudMovie(RankBar);
				await Task.WaitAsync(500);
			}

			Natives.BeginScaleformMovieMethodHudComponent(RankBar, "SET_COLOUR");
			Natives.PushScaleformMovieFunctionParameterInt(color);
			Natives.EndScaleformMovieMethodReturn();

			Natives.BeginScaleformMovieMethodHudComponent(RankBar, "SET_RANK_SCORES");
			Natives.PushScaleformMovieFunctionParameterInt(StartExperience);
			Natives.PushScaleformMovieFunctionParameterInt((int)requiredExperience);
			Natives.PushScaleformMovieFunctionParameterInt((int)previousExperience);
			Natives.PushScaleformMovieFunctionParameterInt((int)currentExperience);
			Natives.PushScaleformMovieFunctionParameterInt((int)currentLevel);
			Natives.PushScaleformMovieFunctionParameterInt(Opasity);
			Natives.EndScaleformMovieMethodReturn();

			if (this.rankBarHidden)
			{
				Task.Run(() =>
				{
					this.rankBarHidden = false;
					GangWarController.Show();
				}, 7000);
			}
		}
	}
}
