using GTANetworkAPI;

namespace Gamemode.Models.Spawn
{
    public class Spawn
    {
        public Vector3 Position { get; }
        public float Heading { get; }

        public Spawn(Vector3 position, float heading)
        {
            this.Position = position;
            this.Heading = heading;
        }
    }
}
