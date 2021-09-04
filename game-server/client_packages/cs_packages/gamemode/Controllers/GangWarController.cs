namespace GamemodeClient.Controllers
{
	using RAGE;
	using RAGE.Ui;

	public class GangWarController : Events.Script
	{
		private const string GangWarPath = "package://cs_packages/gamemode/Frontend/Gang/War/index.html";
		private static HtmlWindow? gangWarCEF;

		public GangWarController()
		{
			Events.Add("InitGangWarUI", this.OnInitGangWarUI);
			Events.Add("StartGangWarUI", this.OnStartGangWarUI);
			Events.Add("CloseGangWarUI", this.OnCloseGangWarUI);
			Events.Add("UpdateGangWarStats", this.OnUpdateGangWarStats);
		}

		private void OnUpdateGangWarStats(object[] args)
		{
			if (gangWarCEF == null) return;

			long ballas = (long)args[0];
			long bloods = (long)args[1];
			long marabunta = (long)args[2];
			long families = (long)args[3];
			long vagos = (long)args[4];

			gangWarCEF.ExecuteJs($"updateStats({ballas},{bloods},{marabunta},{families},{vagos})");
		}

		private void OnInitGangWarUI(object[] args)
		{
			string remainingMs = (string)args[0];

			gangWarCEF = new HtmlWindow(GangWarPath);
			gangWarCEF.Active = true;

			gangWarCEF.ExecuteJs($"onInitGangWar({remainingMs})");
		}

		private void OnStartGangWarUI(object[] args)
		{
			string remainingMs = (string)args[0];
			long targetFractionId = (long)args[1];

			if (gangWarCEF == null)
			{
				gangWarCEF = new HtmlWindow(GangWarPath);
				gangWarCEF.Active = true;
			}

			if (!gangWarCEF.Active)
			{
				gangWarCEF.Active = true;
			}

			gangWarCEF.ExecuteJs($"onStartGangWar({remainingMs}, {targetFractionId})");
		}

		private void OnCloseGangWarUI(object[] args)
		{
			Hide();
			gangWarCEF = null;
		}

		public static bool Hide()
		{
			if (gangWarCEF == null) return false;

			gangWarCEF.Active = false;
			return true;
		}

		public static void Show()
		{
			if (gangWarCEF == null) return;

			gangWarCEF.Active = true;
		}
	}
}
