// <copyright file="LoginRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Authentication.Models
{
	using System.Collections.Generic;
	using GamemodeCommon.Email;

	public class LoginRequest
	{
		public string Email { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string Token { get; set; } = string.Empty;

		public List<string> Validate()
		{
			List<string> invalidFieldNames = new List<string>();

			if (string.IsNullOrEmpty(this.Email) || !EmailUtil.IsValidEmail(this.Email))
			{
				invalidFieldNames.Add("email");
			}

			if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 6 || this.Password.Length > 50)
			{
				invalidFieldNames.Add("password");
			}

			return invalidFieldNames;
		}
	}
}
