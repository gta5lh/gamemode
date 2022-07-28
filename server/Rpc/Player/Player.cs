using System;
using System.Collections.Generic;
using Gamemode.Mechanics.Admin.Models;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GTANetworkAPI;

namespace Rpc.Player
{
	public partial class UnbanRequest
	{
		public UnbanRequest(string staticID, Guid unbannedByID)
		{
			this.StaticID = staticID;
			this.UnbannedByID = unbannedByID.ToString();
		}
	}

	public partial class BanRequest
	{
		public BanRequest(string staticID, string reason, Guid bannedByID, DateTime bannedAt, DateTime bannedUntil)
		{
			this.StaticID = staticID;
			this.BanReason = reason;
			this.BannedByID = bannedByID.ToString();
			this.BannedAt = Timestamp.FromDateTime(bannedAt);
			this.BannedUntil = Timestamp.FromDateTime(bannedUntil);
		}
	}

	public partial class MuteRequest
	{
		public MuteRequest(string staticID, string reason, string mutedByID, DateTime mutedAt, DateTime mutedUntil)
		{
			this.StaticID = staticID;
			this.MuteReason = reason;
			this.MutedByID = mutedByID;
			this.MutedAt = Timestamp.FromDateTime(mutedAt);
			this.MutedUntil = Timestamp.FromDateTime(mutedUntil);
		}
	}

	public partial class UnmuteRequest
	{
		public UnmuteRequest(string staticID, string unmutedByID)
		{
			this.StaticID = staticID;
			this.UnmutedByID = unmutedByID;
		}
	}

	public partial class StaticIDByNameRequest
	{
		public StaticIDByNameRequest(string name)
		{
			this.Name = name;
		}
	}

	public partial class SetAdminRankRequest
	{
		public SetAdminRankRequest(string staticID, AdminRank rank, string setByID)
		{
			this.StaticID = staticID;
			this.Rank = (long)rank;
			this.SetByID = setByID;
		}
	}

	public partial class GiveWeaponRequest
	{
		public GiveWeaponRequest(string staticID, WeaponHash weaponHash, long amount, string givenByID)
		{
			this.StaticID = staticID;
			this.Hash = (long)weaponHash;
			this.Amount = amount;
			this.GivenByID = givenByID;
		}
	}

	public partial class RemoveWeaponRequest
	{
		public RemoveWeaponRequest(string staticID, WeaponHash weaponHash, string removedByID)
		{
			this.StaticID = staticID;
			this.Hash = (long)weaponHash;
			this.RemovedByID = removedByID;
		}
	}
}
