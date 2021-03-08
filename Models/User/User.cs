using Gamemode.Models.Admin;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamemode.Models.User
{
    public class User
    {
        [BsonId]
        public long Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public AdminRank AdminRank { get; set; }

        public MuteState? MuteState { get; set; }
    }
}
