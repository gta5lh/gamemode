// <copyright file="UserRepository.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Gamemode.Repositories.Models;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository
    {
        public static async Task<Gamemode.Repositories.Models.User?> GetByEmailAndPassword(string email, string password)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.Include(u => u.Weapons).Include(u => u.FractionRank).FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return null;
                }

                user.Password = string.Empty;

                return user;
            }
        }

        public static async Task<bool> ExistsById(long id)
        {
            using (var db = new UserContext())
            {
                return await db.Users.AnyAsync(u => u.Id == id);
            }
        }

        public static async Task<Repositories.Models.User> GetAdminRankById(long id)
        {
            using (var db = new UserContext())
            {
                return await db.Users.Select(u => new Repositories.Models.User { Id = u.Id, AdminRankId = u.AdminRankId }).FirstOrDefaultAsync(u => u.Id == id);
            }
        }

        public static async Task<Repositories.Models.User> GetUserByEmailOrUsername(string email, string username)
        {
            using (var db = new UserContext())
            {
                return await db.Users.Select(u => new Repositories.Models.User { Email = u.Email, Name = u.Name }).FirstOrDefaultAsync(u => u.Email == email || u.Name == username);
            }
        }

        public static async Task<long?> GetIdByUsername(string username)
        {
            using (var db = new UserContext())
            {
                User user = await db.Users.Select(u => new Repositories.Models.User { Name = u.Name, Id = u.Id }).FirstOrDefaultAsync(u => u.Name == username);
                if (user == null)
                {
                    return null;
                }

                return user.Id;
            }
        }

        public static async Task<FractionRank> GetFractionRankByFractionAndTier(byte fractionId, byte tier)
        {
            using (var db = new UserContext())
            {
                return await db.FractionRanks.FirstOrDefaultAsync(fr => fr.FractionId == fractionId && fr.Tier == tier);
            }
        }

        public static async Task SetFractionRank(long userId, byte rankId)
        {
            using (var db = new UserContext())
            {
                await db.Database.ExecuteSqlRawAsync($"UPDATE users SET fraction_rank_id={rankId}, current_experience=0 WHERE id = {userId}");
                await db.SaveChangesAsync();
            }
        }

        public static async Task<Repositories.Models.User> CreateUser(Repositories.Models.User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            using (var db = new UserContext())
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return user;
            }
        }

        public static async Task<User> Mute(long id, int duration, string reason, long mutedBy)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.AdminRankId == null);
                if (user == null)
                {
                    return null;
                }

                user.MutedUntil = DateTime.UtcNow.AddMinutes(duration);
                user.MutedAt = DateTime.UtcNow;
                user.MuteReason = reason;
                user.MutedById = mutedBy;
                await db.SaveChangesAsync();
                return user;
            }
        }

        public static async Task<User> Unmute(long id)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.AdminRankId == null && u.MutedUntil != null);
                if (user == null)
                {
                    return null;
                }

                user.MutedUntil = null;
                user.MutedAt = null;
                user.MuteReason = null;
                user.MutedById = null;
                await db.SaveChangesAsync();
                return user;
            }
        }

        public static async Task<Repositories.Models.User> SetAdminRank(long id, Models.Admin.AdminRank rank)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }

                user.AdminRankId = rank != 0 ? (byte?)rank : null;
                await db.SaveChangesAsync();
                return user;
            }
        }

        public static async Task<User?> GiveWeapon(long id, Weapon weapon)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.Include(u => u.Weapons).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }

                Weapon? foundWeapon = null;

                if (user.Weapons != null && user.Weapons.Count > 0)
                {
                    foreach (Weapon w in user.Weapons)
                    {
                        if (w.Hash == weapon.Hash)
                        {
                            foundWeapon = w;
                            break;
                        }
                    }
                }

                if (foundWeapon != null)
                {
                    foundWeapon.Amount += weapon.Amount;
                    if (foundWeapon.Amount < 0)
                    {
                        foundWeapon.Amount = 0;
                    }
                }
                else
                {
                    await db.Weapons.AddAsync(weapon);
                }

                await db.SaveChangesAsync();

                return user;
            }
        }

        public static async Task<User?> RemoveWeapon(long id, WeaponHash weaponHash)
        {
            using (var db = new UserContext())
            {
                Repositories.Models.User user = await db.Users.Include(u => u.Weapons).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }

                db.Weapons.RemoveRange(db.Weapons.Where(w => w.UserId == id && w.Hash == weaponHash));
                await db.SaveChangesAsync();

                return user;
            }
        }

        public static async Task SaveUser(long id, List<Weapon> weapons, short currentExperience)
        {
            using var db = new UserContext();
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                await db.Database.ExecuteSqlRawAsync($"UPDATE users SET current_experience = {currentExperience} WHERE id = {id}");
                await db.Database.ExecuteSqlRawAsync($"DELETE FROM weapon WHERE user_id = {id}");
                await db.Weapons.AddRangeAsync(weapons);
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {

            }
        }

        public static async Task<User> Ban(long id, int duration, string reason, long bannedBy)
        {
            using (var db = new UserContext())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.AdminRankId == null);
                if (user == null)
                {
                    return null;
                }

                user.BannedUntil = DateTime.UtcNow.AddMinutes(duration);
                user.BannedAt = DateTime.UtcNow;
                user.BanReason = reason;
                user.BannedById = bannedBy;
                await db.SaveChangesAsync();
                return user;
            }
        }

        public static async Task<User> Unban(long id)
        {
            using (var db = new UserContext())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id && u.AdminRankId == null && u.BannedUntil != null);
                if (user == null)
                {
                    return null;
                }

                user.BannedUntil = null;
                user.BannedAt = null;
                user.BanReason = null;
                user.BannedById = null;
                await db.SaveChangesAsync();
                return user;
            }
        }
        public static async Task<User> SetFraction(long id, byte fraction, byte rank)
        {
            using (var db = new UserContext())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }

                if (fraction != 0)
                {
                    FractionRank fractionRank = await db.FractionRanks.FirstOrDefaultAsync(f => f.FractionId == fraction && f.Tier == rank);
                    if (fractionRank == null)
                    {
                        return null;
                    }

                    user.FractionId = fraction;
                    user.FractionRankId = fractionRank.Id;
                }
                else
                {
                    user.FractionId = null;
                    user.FractionRankId = null;
                    user.Fraction = null;
                    user.FractionRank = null;
                }

                await db.SaveChangesAsync();
                return user;
            }
        }

        public static void Ping()
        {
            using var db = new UserContext();

            try
            {
                 db.Database.ExecuteSqlRaw($"SELECT current_timestamp");
            }
            catch(Exception)
            {

            }
        }
    }
}
