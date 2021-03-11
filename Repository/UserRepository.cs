// <copyright file="UserRepository.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Gamemode.Logger;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Settings;
    using Gamemode.Models.User;
    using GTANetworkAPI;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Core.Events;

    public class UserRepository
    {
        private static UserRepository userRepository;

        private readonly IMongoCollection<User> users;
        private readonly IMongoCollection<StaticIds> staticIds;
        private readonly IMongoClient client;
        private readonly NLog.ILogger logger = Logger.LogFactory.GetCurrentClassLogger();

        private UserRepository(IUserDatabaseSettings settings)
        {
            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.ConnectionString));
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    if (e.CommandName == "isMaster" || e.CommandName == "buildInfo")
                    {
                        return;
                    }

                    this.logger.Info($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };

            this.client = new MongoClient(mongoClientSettings);
            var database = this.client.GetDatabase(settings.DatabaseName);

            users = database.GetCollection<User>(settings.UsersCollectionName);
            staticIds = database.GetCollection<StaticIds>(settings.StaticIdsCollectionName);
        }

        public static void InitUserRepository(IUserDatabaseSettings settings)
        {
            userRepository = new UserRepository(settings);
        }

        public static Task<User> Get(long id)
        {
            return userRepository.users.Find(user => user.Id == id).Project<User>(Builders<User>.Projection.Exclude(user => user.Password)).FirstOrDefaultAsync();
        }

        public static Task<User> GetUserByEmailOrUsername(string email, string username)
        {
            return userRepository.users.Find(user => user.Email == email || user.Username == username).Project<User>(Builders<User>.Projection.Exclude(user => user.Password)).FirstOrDefaultAsync();
        }

        public static async Task<User> GetByEmailAndPassword(string email, string password)
        {
            User user = await userRepository.users.Find(user => user.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            user.Password = null;

            return user;
        }

        public static async Task<long?> GetIdByUsername(string username)
        {
            User user = await userRepository.users.Find(user => user.Username == username).Project<User>(Builders<User>.Projection.Include(user => user.Id)).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            return user.Id;
        }


        public static async Task<User> CreateUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            using (var session = await userRepository.client.StartSessionAsync())
            {
                session.StartTransaction();

                try
                {
                    var update = new UpdateDefinitionBuilder<StaticIds>().Inc(staticIds => staticIds.Sequence, 1);
                    var opts = new FindOneAndUpdateOptions<StaticIds>();
                    opts.ReturnDocument = ReturnDocument.After;
                    opts.Projection = new ProjectionDefinitionBuilder<StaticIds>().Exclude(staticIds => staticIds.Id);

                    var staticId = await userRepository.staticIds.FindOneAndUpdateAsync<StaticIds>(session, staticIds => staticIds.Id == "userSequence", update, opts);

                    user.Id = staticId.Sequence;
                    await userRepository.users.InsertOneAsync(session, user);

                    await session.CommitTransactionAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing to MongoDB: " + e.Message);
                    await session.AbortTransactionAsync();
                    return null;
                }
            }

            user.Password = null;

            return user;
        }

        public static Task<User> Mute(long targetId, int duration, string reason, long mutedBy)
        {
            MuteState muteState = new MuteState(duration, mutedBy, reason);
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.MuteState, muteState);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId && user.AdminRank == 0, update, opts);
        }

        public static Task<User> Unmute(long targetId)
        {
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.MuteState, null);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId && user.AdminRank == 0 && user.MuteState != null, update, opts);
        }

        public static Task<User> SetAdminRank(long targetId, AdminRank rank)
        {
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.AdminRank, rank);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId, update, opts);
        }

        public static Task<User> GiveWeapon(long targetId, Weapon weapon)
        {
            var update = new UpdateDefinitionBuilder<User>().Push(user => user.Weapons, weapon);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId, update, opts);
        }

        public static Task<User> RemoveWeapon(long targetId, WeaponHash weaponHash)
        {
            var update = new UpdateDefinitionBuilder<User>().PullFilter(user => user.Weapons, weapon => weapon.WeaponHash == weaponHash);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId, update, opts);
        }

        public static Task UpdateWeapons(long targetId, List<Weapon> weapons)
        {
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.Weapons, weapons.ToArray());
            return userRepository.users.UpdateOneAsync(user => user.Id == targetId, update);
        }

        public static Task<User> Ban(long targetId, int duration, string reason, long bannedBy)
        {
            BanState banState = new BanState(duration, bannedBy, reason);
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.BanState, banState);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId && user.AdminRank == 0, update, opts);
        }

        public static Task<User> Unban(long targetId)
        {
            var update = new UpdateDefinitionBuilder<User>().Set(user => user.BanState, null);
            var opts = new FindOneAndUpdateOptions<User>();
            opts.ReturnDocument = ReturnDocument.After;
            opts.Projection = new ProjectionDefinitionBuilder<User>().Exclude(user => user.Password);
            return userRepository.users.FindOneAndUpdateAsync<User>(user => user.Id == targetId && user.AdminRank == 0 && user.BanState != null, update, opts);
        }
    }
}
