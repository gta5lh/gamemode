using System.Collections.Generic;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Models.Player
{
    public class FractionsCache
    {
        private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
        private static readonly Dictionary<byte, Dictionary<long, string>> FractionMembers = new Dictionary<byte, Dictionary<long, string>>()
        {
            { 1, new Dictionary<long, string>() },
            { 2, new Dictionary<long, string>() },
            { 3, new Dictionary<long, string>() },
            { 4, new Dictionary<long, string>() },
            { 5, new Dictionary<long, string>() }
        };

        public static void LoadFractionMemberToCache(byte fractionId, long staticId, string name)
        {
            if (FractionMembers[fractionId].ContainsKey(staticId))
            {
                return;
            }

            FractionMembers[fractionId][staticId] = name;
            Logger.Info($"Loaded fraction member to cache. static_id={staticId}");
        }

        public static void UnloadFractionMemberFromCache(byte fractionId, long staticId)
        {
            if (!FractionMembers[fractionId].ContainsKey(staticId))
            {
                return;
            }

            FractionMembers[fractionId].Remove(staticId, out _);
            Logger.Info($"Unloaded fraction member from cache. static_id={staticId}");
        }

        public static void SendMessageToAllFractionMembers(byte fractionId, string message)
        {
            foreach (long staticId in FractionMembers[fractionId].Keys)
            {
                NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(staticId), $"{ChatColor.FractionChatColor}[F] {message}");
            }
        }
    }
}
