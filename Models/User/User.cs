using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Gamemode.Models.User
{
    public class User
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
