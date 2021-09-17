using RAGE.Ui;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public delegate void disableUIStateChangedDelegate(bool enabled);
		public static event disableUIStateChangedDelegate disableUIStateChangedEvent;

		private const string IndexPath = "package://cs_packages/gamemode/Frontend/v2/build/index.html";

		public static HtmlWindow IndexCef { get; private set; }

		static Cef()
		{
			IndexCef = new HtmlWindow(IndexPath);
			IndexCef.Active = true;
		}
	}
}
