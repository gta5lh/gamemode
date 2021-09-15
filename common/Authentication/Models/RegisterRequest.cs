// <copyright file="RegisterRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeCommon.Authentication.Models
{
	using System.Collections.Generic;
	using GamemodeCommon.Email;

	public class RegisterRequest
	{
		public string Email { get; set; } = string.Empty;

		public string Username { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string ConfirmPassword { get; set; } = string.Empty;

		public List<string> Validate()
		{
			List<string> invalidFieldNames = new List<string>();

			if (string.IsNullOrEmpty(this.Email) || !EmailUtil.IsValidEmail(this.Email))
			{
				invalidFieldNames.Add("email");
			}

			if (string.IsNullOrEmpty(this.Username) || this.Username.Length < 1 || this.Username.Length > 32)
			{
				invalidFieldNames.Add("username");
			}

			if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 6 || this.Password.Length > 50)
			{
				invalidFieldNames.Add("password");
			}

			if (string.IsNullOrEmpty(this.ConfirmPassword) || !this.Password.Equals(this.ConfirmPassword))
			{
				invalidFieldNames.Add("confirm_password");
			}

			return invalidFieldNames;
		}
	}
}
