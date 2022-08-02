// <copyright file="Natives.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.Natives
{
	public static class Natives
	{
		public static void RequestScaleformScriptHudMovie(int hudComponent)
		{
			RAGE.Game.Invoker.Invoke(0x9304881D6F6537EA, hudComponent);
		}

		public static bool HasScaleformScriptHudMovieLoaded(int hudComponent)
		{
			int loaded = RAGE.Game.Invoker.Invoke<int>(0xDF6E5987D2B4D140, hudComponent);
			return loaded > 0;
		}

		public static void BeginScaleformMovieMethodHudComponent(int hudComponent, string methodName)
		{
			RAGE.Game.Invoker.Invoke(0x98C494FD5BDFBFD5, hudComponent, methodName);
		}

		public static void PushScaleformMovieFunctionParameterInt(int value)
		{
			RAGE.Game.Invoker.Invoke(0xC3D0841A0CC546A6, value);
		}

		public static void EndScaleformMovieMethodReturn()
		{
			RAGE.Game.Invoker.Invoke(0xC50AA39A577AF886);
		}

		public static void DisplayHelpText(string msg)
		{
			RAGE.Game.Ui.BeginTextCommandDisplayHelp("STRING");
			RAGE.Game.Ui.AddTextComponentSubstringPlayerName(msg);
			RAGE.Game.Ui.EndTextCommandDisplayHelp(0, false, true, -1);
		}

		public static void SetThisScripCanRemoveBlipsCreatedByAnyScript(bool toggle)
		{
			RAGE.Game.Invoker.Invoke(0xB98236CAAECEF897, true);
		}
	}
}
