// <copyright file="Register.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using GTANetworkAPI;
using System.Threading.Tasks;

namespace Gamemode.Commands.Player
{
    public class RegisterCommand : Script
    {
        [Command("register", "Usage: /register {адрес электронной почты} {имя} {пароль} {подтверждение_пароль}", SensitiveInfo = true, GreedyArg = true)]
        public async Task Register(GTANetworkAPI.Player player, string email, string name, string password, string confirmPassword)
        {
            if (!IsValidEmail(email))
            {
                player.SendChatMessage("Введите свой адрес электронной почты");
                return;
            }

            if (name.Length == 0 || name.Length > 32)
            {
                player.SendChatMessage("Имя должно содержать минимум 1 символ и максимум 32");
                return;
            }

            if (!password.Equals(confirmPassword))
            {
                player.SendChatMessage("Пароли должны совпадать");
                return;
            }

            //bool exists = await UserRepository.Instance.UserByEmailExists(email);
            //if (exists)
            //{
            //    NAPI.Task.Run(() => player.SendChatMessage("Введенный адрес электронной почты уже занят"));
            //}
        }

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
