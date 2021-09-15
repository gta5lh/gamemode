using System;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void ShowHud()
		{
			IndexCef.ExecuteJs($"ShowHud()");
		}

		public static void HideHud()
		{
			IndexCef.ExecuteJs($"HideHud()");
		}

		public static void UpdateOnline()
		{
			IndexCef.ExecuteJs($"UpdateOnline({RAGE.Elements.Entities.Players.Count})");
		}

		public static void UpdateSpeedometer(int speed)
		{
			IndexCef.ExecuteJs($"UpdateSpeedometer({speed})");
		}

		public static void ShowSpeedometer()
		{
			IndexCef.ExecuteJs($"ShowSpeedometer()");
		}

		public static void HideSpeedometer()
		{
			IndexCef.ExecuteJs($"HideSpeedometer()");
		}

		public static void ShowVoice()
		{
			IndexCef.ExecuteJs($"ShowVoice()");
		}

		public static void HideVoice()
		{
			IndexCef.ExecuteJs($"HideVoice()");
		}

		public static void UpdateMoney(long money)
		{
			IndexCef.ExecuteJs($"UpdateMoney({money})");
		}

		public static void UpdateTime(int hours, int minutes, int day, int month)
		{
			IndexCef.ExecuteJs($"UpdateTime('{hours:00.##}', '{minutes:00.##}', '{day:00.##}', '{month:00.##}')");
		}

		public static void HideHelpMenu()
		{
			IndexCef.ExecuteJs("HideHelpMenu()");
		}

		public static void ShowHelpMenu()
		{
			IndexCef.ExecuteJs("ShowHelpMenu()");
		}

		public static void SetZoneState(bool isInZone, string color)
		{
			IndexCef.ExecuteJs($"SetZoneState({isInZone.ToString().ToLower()}, '{color}')");
		}
	}
}
