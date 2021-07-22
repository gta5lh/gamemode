using RAGE.Ui;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		private const string IndexPath = "package://cs_packages/gamemode/Frontend/v2/build/index.html";

		private static readonly HtmlWindow IndexCef = new HtmlWindow(IndexPath);
	}
}
