﻿namespace Gamemode.Controllers
{
    using System;
    using System.Timers;
    using Gamemode.Models.Player;
    using Gamemode.Services.Player;
    using GTANetworkAPI;

    public class PaydayController : Script
    {

        private static Timer PaydayTimer;

        private static readonly double PaydayInterval30Minutes = 1000 * 60 * 30;
        private static readonly double PaydayAllowedLeeway = -(1000 * 60);
        //private static readonly double PaydayInterval30Minutes = 10000;
        //private static readonly double PaydayAllowedLeeway = -2000;

        public static void SetPaydayTimer()
        {
            PaydayTimer = new System.Timers.Timer(PaydayInterval30Minutes);
            PaydayTimer.Elapsed += OnPaydayTime;
            PaydayTimer.AutoReset = true;
            PaydayTimer.Enabled = true;
        }

        private static void OnPaydayTime(object source, ElapsedEventArgs e)
        {
            NAPI.Task.Run(() =>
            {
                DateTime paydayTime = DateTime.UtcNow.AddMilliseconds(PaydayAllowedLeeway);
                NAPI.Chat.SendChatMessageToAll($"{paydayTime.ToLongTimeString()} Время зарплаты и получения опыта!");

                foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
                {
                    if (player.Fraction == null || player.LoggedInAt == null)
                    {
                        continue;
                    }

                    if (DateTime.Compare(player.LoggedInAt.Value.AddMilliseconds(PaydayInterval30Minutes), paydayTime) >= 1)
                    {
                        continue;
                    }

                    long salary = GangUtil.SalaryByRank[player.FractionRank.Value];
                    player.Money += salary;
                    ExperienceService.ChangeExperience(player, 3);
                    player.SendChatMessage($"Вы получили: {salary} вирт");
                }
            });
        }
    }
}
