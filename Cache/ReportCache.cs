using System.Collections.Concurrent;
using System.Collections.Generic;
using Gamemode.ApiClient.Models;

namespace Gamemode.Models.Player
{
    public class ReportCache
    {
        private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<long, ReportCacheEntity> Reports = new ConcurrentDictionary<long, ReportCacheEntity>();

        public static void LoadReportsToCache(List<ApiClient.Models.Report> reports)
        {
            foreach (ApiClient.Models.Report report in reports)
            {
                Reports[report.Id.Value] = new ReportCacheEntity(report);
            }

            Logger.Info($"Loaded reports to cache.");
        }

        public static ICollection<ReportCacheEntity> GetAll()
        {
            return Reports.Values;
        }

        public static void AddReport(Report report)
        {
            Reports.TryAdd(report.Id.Value, new ReportCacheEntity(report));
        }

        //public static void LoadAdminToCache(long staticId, string name)
        //{
        //    if (Admins.ContainsKey(staticId))
        //    {
        //        return;
        //    }

        //    Admins[staticId] = name;
        //    Logger.Info($"Loaded admin to cache. static_id={staticId}");
        //}

        //public static void UnloadAdminFromCache(long staticId)
        //{
        //    if (!Admins.ContainsKey(staticId))
        //    {
        //        return;
        //    }

        //    Admins.TryRemove(staticId, out _);
        //    Logger.Info($"Unloaded admin from cache. static_id={staticId}");
        //}
    }

    public class ReportCacheEntity
    {
        public ReportCacheEntity(Report report)
        {
            Report = report;
            ReportAnswers = new List<ReportAnswer>();
        }

        public ApiClient.Models.Report Report { get; set; }

        public List<ApiClient.Models.ReportAnswer> ReportAnswers { get; set; }
    }
}
