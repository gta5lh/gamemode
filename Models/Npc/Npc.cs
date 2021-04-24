using GTANetworkAPI;

namespace Gamemode.Models.Npc
{
    public class Npc
    {
        public Vector3 Position { get; }
        public float Heading { get; }
        public string Name { get; }
        public PedHash Model { get; }

        public Npc(Vector3 position, float heading, string name, PedHash model)
        {
            this.Position = position;
            this.Heading = heading;
            this.Name = name;
            this.Model = model;
        }

        public void Create()
        {
            NAPI.Ped.CreatePed((uint)this.Model, this.Position, this.Heading, false, true, true);
        }
    }
}
