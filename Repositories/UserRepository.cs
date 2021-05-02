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

        public static async Task<Repositories.Models.User> GetAdminRankById(long id)
        {
            using (var db = new UserContext())
            {
                return await db.Users.Select(u => new Repositories.Models.User { Id = u.Id, AdminRankId = u.AdminRankId }).FirstOrDefaultAsync(u => u.Id == id);
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

        public static void Ping()
        {
            using var db = new UserContext();

            try
            {
                db.Database.ExecuteSqlRaw($"SELECT current_timestamp");
            }
            catch (Exception)
            {

            }
        }
    }
}
