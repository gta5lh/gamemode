using System;
using System.Collections.Generic;
using Gamemode.Models.Admin;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GTANetworkAPI;

namespace Rpc.User
{
	public partial class UnbanRequest
	{
		public UnbanRequest(long id, long unbannedBy)
		{
			this.ID = id;
			this.UnbannedBy = unbannedBy;
		}
	}

	public partial class BanRequest
	{
		public BanRequest(long id, string reason, long bannedBy, DateTime bannedAt, DateTime bannedUntil)
		{
			this.ID = id;
			this.BanReason = reason;
			this.BannedByID = bannedBy;
			this.BannedAt = Timestamp.FromDateTime(bannedAt);
			this.BannedUntil = Timestamp.FromDateTime(bannedUntil);
		}
	}

	public partial class Weapon
	{
		public Weapon(WeaponHash hash, long amount)
		{
			this.Hash = (long)hash;
			this.Amount = amount;
		}
	}

	public partial class SaveRequest
	{
		public SaveRequest(long id, long experience, long money, List<Weapon> weapons, long health, long armor)
		{
			this.ID = id;
			this.Experience = experience;
			this.Money = money;
			this.Weapons.Add(weapons);
			this.Health = health;
			this.Armor = armor;
		}
	}

	public partial class SaveAllRequest
	{
		public SaveAllRequest(List<SaveRequest> saveRequests)
		{
			this.SaveRequests.Add(saveRequests);
		}
	}

	public partial class LoginRequest
	{
		public LoginRequest(string email, string password, string lastIPAddress, string socialClubID, string computerSerialNumber, GameTypes gameLauncherID, string token)
		{
			this.Email = email;
			this.Password = password;
			this.LastIPAddress = lastIPAddress;
			this.SocialClubID = socialClubID;
			this.ComputerSerialNumber = computerSerialNumber;
			this.GameLauncherID = (long)gameLauncherID;
			this.Token = token;
		}
	}

	public partial class RegisterRequest
	{
		public RegisterRequest(string email, string name, string password, string lastIPAddress, string socialClubID, string computerSerialNumber, GameTypes gameLauncherID)
		{
			this.Email = email;
			this.Name = name;
			this.Password = password;
			this.LastIPAddress = lastIPAddress;
			this.SocialClubID = socialClubID;
			this.ComputerSerialNumber = computerSerialNumber;
			this.GameLauncherID = (long)gameLauncherID;
		}
	}

	public partial class SetAdminRankRequest
	{
		public SetAdminRankRequest(long id, AdminRank rank, long setBy)
		{
			this.ID = id;
			this.Rank = (long)rank;
			this.SetBy = setBy;
		}
	}

	public partial class MuteRequest
	{
		public MuteRequest(long id, string reason, long mutedBy, DateTime mutedAt, DateTime mutedUntil)
		{
			this.ID = id;
			this.MuteReason = reason;
			this.MutedByID = mutedBy;
			this.MutedAt = Timestamp.FromDateTime(mutedAt);
			this.MutedUntil = Timestamp.FromDateTime(mutedUntil);
		}
	}

	public partial class UnmuteRequest
	{
		public UnmuteRequest(long id, long unmutedBy)
		{
			this.ID = id;
			this.UnmutedBy = unmutedBy;
		}
	}

	public partial class GiveWeaponRequest
	{
		public GiveWeaponRequest(long id, WeaponHash weaponHash, long amount, long givenBy)
		{
			this.ID = id;
			this.Hash = (long)weaponHash;
			this.Amount = amount;
			this.GivenBy = givenBy;
		}
	}

	public partial class RemoveWeaponRequest
	{
		public RemoveWeaponRequest(long id, WeaponHash weaponHash, long removedBy)
		{
			this.ID = id;
			this.Hash = (long)weaponHash;
			this.RemovedBy = removedBy;
		}
	}

	public partial class SetFractionRequest
	{
		public SetFractionRequest(long id, long fractionID, long tier, long setBy)
		{
			this.ID = id;
			this.Fraction = fractionID;
			this.Tier = tier;
			this.SetBy = setBy;
		}
	}

	public partial class IDByNameRequest
	{
		public IDByNameRequest(string name)
		{
			this.Name = name;
		}
	}

	public partial class LogoutRequest
	{
		public LogoutRequest(long id, long money, long experience, List<Weapon> weapons, long health, long armor)
		{
			this.ID = id;
			this.Money = money;
			this.Experience = experience;
			this.Weapons.Add(weapons);
			this.Health = health;
			this.Armor = armor;
		}
	}

	public partial class RequestResetPasswordRequest
	{
		public RequestResetPasswordRequest(string email)
		{
			this.Email = email;
		}
	}
}
