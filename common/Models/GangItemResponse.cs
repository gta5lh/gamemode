// <copyright file="GangItemResponse.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Models
{
	public class GangItemResponse
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GangItemResponse"/> class.
		/// </summary>
		/// <param name="notificationType"></param>
		/// <param name="text"></param>
		public GangItemResponse(string notificationType, string text)
		{
			this.NotificationType = notificationType;
			this.Text = text;
		}

		public string NotificationType { get; set; }

		public string Text { get; set; }
	}
}
