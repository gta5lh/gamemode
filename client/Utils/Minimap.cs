namespace GamemodeClient.Utils
{
	using RAGE.Game;
	using System;

	public class Minimap
	{
		public float width, height, left_x, bottom_y, right_x, top_y,
			x, y, xunit, yunit;

		public static Minimap GetMinimapAnchor()
		{
			float safezone = Graphics.GetSafeZoneSize();
			float safezone_x = 1.0f / 20.0f;
			float safezone_y = 1.0f / 20.0f;
			float aspect_ratio = Graphics.GetAspectRatio(false);
			if (aspect_ratio > 2)
			{
				aspect_ratio = 16 / 9;
			}

			int res_x = 0, res_y = 0;
			Graphics.GetActiveScreenResolution(ref res_x, ref res_y);
			float xscale = 1.0f / res_x;
			float yscale = 1.0f / res_y;

			Minimap minimap = new Minimap();

			minimap.width = xscale * (res_x / (4 * aspect_ratio));
			minimap.height = yscale * (res_y / 5.674f);
			minimap.left_x = xscale * (res_x * (safezone_x * (Math.Abs(safezone - 1.0f) * 10)));

			if (Graphics.GetAspectRatio(false) > 2)
			{
				minimap.left_x += minimap.width * 0.845f;
				minimap.width *= 0.76f;
			}
			else if (Graphics.GetAspectRatio(false) > 1.8)
			{
				minimap.left_x += minimap.width * 0.2225f;
				minimap.width *= 0.995f;
			}

			minimap.bottom_y = 1.0f - yscale * (res_y * (safezone_y * ((Math.Abs(safezone - 1.0f)) * 10)));
			minimap.right_x = minimap.left_x + minimap.width;
			minimap.top_y = minimap.bottom_y - minimap.height;
			minimap.x = res_x * minimap.left_x;
			minimap.y = res_y - (res_y * minimap.top_y);
			minimap.xunit = xscale;
			minimap.yunit = yscale;

			return minimap;
		}
	}
}
