// <copyright file="Register.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Player
{
    using GTANetworkAPI;
    using System.Threading.Tasks;

    public class RegisterCommand : Script
    {
        [Command("n", "Usage: /register {адрес электронной почты} {имя} {пароль} {подтверждение_пароль}", SensitiveInfo = true, GreedyArg = true)]
        public async Task Na(GTANetworkAPI.Player player, string name)
        {
            player.Name = name;
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
