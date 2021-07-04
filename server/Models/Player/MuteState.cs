// <copyright file="MuteState.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;

	public class MuteState
	{
		public DateTime? MutedUntil { get; set; }

		public string? Reason { get; set; }

		public long? MutedBy { get; set; }

		public MuteState()
		{
			this.MutedUntil = null;
			this.MutedBy = null;
			this.Reason = null;
		}


		public MuteState(int? duration = null, long? mutedBy = null, string? reason = null)
		{
			this.MutedUntil = duration.HasValue ? (DateTime?)DateTime.UtcNow.AddMinutes(duration.Value) : null;
			this.MutedBy = mutedBy;
			this.Reason = reason;
		}

		public MuteState(DateTime? mutedUntil = null, long? mutedBy = null, string? reason = null)
		{
			this.MutedUntil = mutedUntil;
			this.MutedBy = mutedBy;
			this.Reason = reason;
		}

		public bool IsMuted()
		{
			return this.MutedUntil != null;
		}

		public bool HasMuteExpired()
		{
			return DateTime.Compare(DateTime.UtcNow, (DateTime)this.MutedUntil) >= 0;
		}

		public void Mute(int durationMinutes, long mutedBy, string reason)
		{
			this.MutedUntil = DateTime.UtcNow.AddMinutes(durationMinutes);
			this.MutedBy = mutedBy;
			this.Reason = reason;
		}

		public void Unmute()
		{
			this.MutedUntil = null;
			this.MutedBy = null;
			this.Reason = null;
		}

		public double GetMinutesLeft()
		{
			return ((TimeSpan)(this.MutedUntil - DateTime.UtcNow)).TotalMinutes;
		}
	}
}
