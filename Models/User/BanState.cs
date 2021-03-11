// <copyright file="BanState.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Models.User
{
    using System;

    public class BanState
    {
        public DateTime BannedUntil { get; set; }

        public string Reason { get; set; }

        public long BannedBy { get; set; }

        public BanState(int duration, long bannedBy, string reason)
        {
            this.BannedUntil = DateTime.UtcNow.AddDays(duration);
            this.BannedBy = bannedBy;
            this.Reason = reason;
        }
    }
}
