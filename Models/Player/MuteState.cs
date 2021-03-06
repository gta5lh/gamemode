// <copyright file="MuteState.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System;

    public class MuteState
    {
        private DateTime? mutedUntil;

        public bool IsMuted()
        {
            return this.mutedUntil != null;
        }

        public bool HasMuteExpired()
        {
            return DateTime.Compare(DateTime.UtcNow, (DateTime)this.mutedUntil) >= 0;
        }

        public void Mute(int durationMinutes)
        {
            this.mutedUntil = DateTime.UtcNow.AddMinutes(durationMinutes);
        }

        public void Unmute()
        {
            this.mutedUntil = null;
        }
    }
}
