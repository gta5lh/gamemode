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
			int previousExperience = (int)args[0];
			int currentExperience = (int)args[1];
			int requiredExperience = (int)args[2];
			int currentLevel = (int)args[3];

			this.DisplayRankBar(requiredExperience, previousExperience, currentExperience, currentLevel);
		}

		private bool rankBarHidden = false;

		private async void DisplayRankBar(int requiredExperience, int previousExperience, int currentExperience, int currentLevel)
		{
			if (rankBarHidden) return;

			if (GangWarController.Hide()) rankBarHidden = true;

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
			Natives.PushScaleformMovieFunctionParameterInt(requiredExperience);
			Natives.PushScaleformMovieFunctionParameterInt(previousExperience);
			Natives.PushScaleformMovieFunctionParameterInt(currentExperience);
			Natives.PushScaleformMovieFunctionParameterInt(currentLevel);
			Natives.PushScaleformMovieFunctionParameterInt(Opasity);
			Natives.EndScaleformMovieMethodReturn();

			if (rankBarHidden)
			{
				Task.Run(() =>
				{
					rankBarHidden = false;
					GangWarController.Show();
				}, 7000);
			}
		}
	}
}
