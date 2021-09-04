// <copyright file="RegisterRequest.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeCommon.Authentication.Models
{
	using System.Collections.Generic;
	using GamemodeCommon.Email;

	public class ResetPasswordRequest
	{
		public string Email { get; set; }

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
