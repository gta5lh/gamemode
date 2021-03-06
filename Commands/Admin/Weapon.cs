// <copyright file="Weapon.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System.Text;
    using GTANetworkAPI;

    public class Weapon : Script
    {
        [Command("weapon", "Usage: /weapon {название}. Название может содержать множество оружий через запятую.", Alias = "w", SensitiveInfo = true, GreedyArg = true)]
        public void CMDWeapon(Player player, string weaponNames)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string name in weaponNames.Replace(", ", ",").Split(","))
            {
                WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(name);
                if (weaponHash == 0)
                {
                    stringBuilder.Append(string.Format("[{0}], ", name));
                }

                player.GiveWeapon(weaponHash, 100);
            }

            if (stringBuilder.Length > 0)
            {
                string unknownWeaponNames = stringBuilder.ToString();
                player.SendChatMessage("Неизвестные названия оружий: " + unknownWeaponNames.Remove(unknownWeaponNames.Length - 2));
                return;
            }

            player.SendChatMessage("Вы выдали себе оружие: " + weaponNames);
        }
    }
}
