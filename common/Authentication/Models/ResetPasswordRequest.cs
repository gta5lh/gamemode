// <copyright file="ResetPasswordRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Authentication.Models
{
	using System.Collections.Generic;
	using GamemodeCommon.Email;

	public class ResetPasswordRequest
	{
		public string Email { get; set; } = string.Empty;

		public List<string> Validate()
		{
			List<string> invalidFieldNames = new List<string>();

			if (string.IsNullOrEmpty(this.Email) || !EmailUtil.IsValidEmail(this.Email))
			{
				invalidFieldNames.Add("email");
			}

			return invalidFieldNames;
		}
	}
}
