// <copyright file="GangItemResponse.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Models
{
	public class GangItemResponse
	{
		public GangItemResponse(string notificationType, string text)
		{
			this.NotificationType = notificationType;
			this.Text = text;
		}

		public string NotificationType { get; set; }

		public string Text { get; set; }
	}
}
