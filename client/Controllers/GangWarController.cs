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
			Events.Add("CloseGangWarUI", this.OnCloseGangWarUI);
			Events.Add("UpdateGangWarStats", this.OnUpdateGangWarStats);
		}

		private void OnUpdateGangWarStats(object[] args)
		{
			if (gangWarCEF == null) return;

			int ballas = (int)args[0];
			int bloods = (int)args[1];
			int marabunta = (int)args[2];
			int families = (int)args[3];
			int vagos = (int)args[4];

			gangWarCEF.ExecuteJs($"updateStats({ballas},{bloods},{marabunta},{families},{vagos})");
		}
		private void OnInitGangWarUI(object[] args)
		{
			string remainingMs = (string)args[0];
			long targetFractionId = (long)args[1];

			gangWarCEF = new HtmlWindow(GangWarPath);
			gangWarCEF.Active = true;

			gangWarCEF.ExecuteJs($"initStats({remainingMs}, {targetFractionId})");
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
