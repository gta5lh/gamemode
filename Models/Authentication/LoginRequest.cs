// <copyright file="LoginRequest.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.Authentication
{
    using System.Collections.Generic;

    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Validate()
        {
            List<string> invalidFieldNames = new List<string>();

            if (string.IsNullOrEmpty(this.Email) || !IsValidEmail(this.Email))
            {
                invalidFieldNames.Add("email");
            }

            if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 6 || this.Password.Length > 72)
            {
                invalidFieldNames.Add("password");
            }

            return invalidFieldNames;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
