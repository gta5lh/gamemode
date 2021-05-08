namespace Gamemode.Controllers
{
    using System;
    using System.Timers;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class MoneyController : Script
    {
        [ServerEvent(Event.PlayerDeath)]
        private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
        {
            if (killer == null)
            {
                return;
            }

            if (target.Fraction == null)
            {
                // TODO: Punish if newby killed?
                return;
            }

            if (killer.Fraction == null)
            {
                // TODO: Punish if newby killed gang member?
                return;
            }

            long reward = GangUtil.RewardByRank[target.FractionRank.Value];

            if (killer.Fraction == target.Fraction)
            {
                killer.Money -= reward;
            }
            else
            {
                killer.Money += reward;
            }
        }

        private static Timer PaydayTimer;

        private static readonly double PaydayInterval30Minutes = 1000 * 60 * 30;
        private static readonly double PaydayAllowedLeeway = -(1000 * 60);

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
                NAPI.Chat.SendChatMessageToAll($"{paydayTime.ToLongTimeString()} Время зарплаты!");

                foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
                {
                    if (player.Fraction == null || player.LoggedInAt == null)
                    {
                        return;
                    }

                    if (DateTime.Compare(player.LoggedInAt.Value.AddMilliseconds(PaydayInterval30Minutes), paydayTime) >= 1)
                    {
                        continue;
                    }

                    long salary = GangUtil.SalaryByRank[player.FractionRank.Value];
                    player.Money += salary;
                    player.SendChatMessage($"Вы получили: {salary}");
                }
            });
        }
    }
}
