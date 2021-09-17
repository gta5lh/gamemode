using GamemodeClient.Models;
using Newtonsoft.Json;
using RAGE;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void UpdateExperience(UpdateExperience updateExperience)
		{
			string updateExperienceJson = JsonConvert.SerializeObject(updateExperience);
			IndexCef.ExecuteJs($"UpdateExperience('{updateExperienceJson}')");
		}

		public static void SetXAndY()
		{
			Utils.Minimap minimap = Utils.Minimap.GetMinimapAnchor();
			string setXAndYJson = JsonConvert.SerializeObject(new SetXAndY(minimap.x, minimap.y));
			IndexCef.ExecuteJs($"SetXAndY('{setXAndYJson}')");
		}
	}

	public class SetXAndY
	{

		[JsonProperty("x")]
		public double X;


		[JsonProperty("y")]
		public double Y;

		public SetXAndY(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
