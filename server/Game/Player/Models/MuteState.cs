// <copyright file="MuteState.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System;

	public class MuteState
	{
		public DateTime? MutedUntil { get; set; }

		public string? Reason { get; set; }

		public string? MutedByID { get; set; }

		public MuteState()
		{
			this.MutedUntil = null;
			this.MutedByID = null;
			this.Reason = null;
		}

		public MuteState(int? duration = null, string? mutedByID = null, string? reason = null)
		{
			this.MutedUntil = duration.HasValue ? (DateTime?)DateTime.UtcNow.AddMinutes(duration.Value) : null;
			this.MutedByID = mutedByID;
			this.Reason = reason;
		}

		public MuteState(DateTime? mutedUntil = null, string? mutedByID = null, string? reason = null)
		{
			this.MutedUntil = mutedUntil;
			this.MutedByID = mutedByID;
			this.Reason = reason;
		}

		public bool IsMuted()
		{
			return this.MutedUntil != null;
		}

		public bool HasMuteExpired()
		{
			if (!this.MutedUntil.HasValue)
			{
				return true;
			}

			return DateTime.Compare(DateTime.UtcNow, (DateTime)this.MutedUntil) >= 0;
		}

		public void Mute(int durationMinutes, string mutedByID, string reason)
		{
			this.MutedUntil = DateTime.UtcNow.AddMinutes(durationMinutes);
			this.MutedByID = mutedByID;
			this.Reason = reason;
		}

		public void Unmute()
		{
			this.MutedUntil = null;
			this.MutedByID = null;
			this.Reason = null;
		}

		public double GetMinutesLeft()
		{
			if (this.MutedUntil == null)
			{
				return 0;
			}

			return ((TimeSpan)(this.MutedUntil - DateTime.UtcNow)).TotalMinutes;
		}
	}
}
