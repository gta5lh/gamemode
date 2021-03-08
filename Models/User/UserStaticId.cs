using MongoDB.Bson.Serialization.Attributes;

namespace Gamemode.Models.User
{
    public class StaticIds
    {
        [BsonId]
        public string Id { get; set; }

        public long Sequence { get; set; }
    }
}
