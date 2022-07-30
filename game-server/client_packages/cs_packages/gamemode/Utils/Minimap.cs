// <copyright file="Minimap.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Utils
{
	using System;
	using RAGE.Game;

	public class Minimap
	{
		public float Width;
		public float Height;
		public float LeftX;
		public float BottomY;
		public float RightX;
		public float TopY;
		public float X;
		public float Y;
		public float Xunit;
		public float Yunit;

		public static Minimap GetMinimapAnchor()
		{
			float safezone = Graphics.GetSafeZoneSize();
			const float safezone_x = 1.0f / 20.0f;
			const float safezone_y = 1.0f / 20.0f;
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

			minimap.Width = xscale * (res_x / (4 * aspect_ratio));
			minimap.Height = yscale * (res_y / 5.674f);
			minimap.LeftX = xscale * (res_x * (safezone_x * (Math.Abs(safezone - 1.0f) * 10)));

			if (Graphics.GetAspectRatio(false) > 2)
			{
				minimap.LeftX += minimap.Width * 0.845f;
				minimap.Width *= 0.76f;
			}
			else if (Graphics.GetAspectRatio(false) > 1.8)
			{
				minimap.LeftX += minimap.Width * 0.2225f;
				minimap.Width *= 0.995f;
			}

			minimap.BottomY = 1.0f - (yscale * (res_y * (safezone_y * (Math.Abs(safezone - 1.0f) * 10))));
			minimap.RightX = minimap.LeftX + minimap.Width;
			minimap.TopY = minimap.BottomY - minimap.Height;
			minimap.X = res_x * minimap.LeftX;
			minimap.Y = res_y - (res_y * minimap.TopY);
			minimap.Xunit = xscale;
			minimap.Yunit = yscale;

			return minimap;
		}
	}
}
