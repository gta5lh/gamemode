namespace Gamemode.Models.Vehicle
{
    using GTANetworkAPI;

    public class CustomVehicle : Vehicle
    {
        public long OwnerPlayerId { get; set; }

        public CustomVehicle(NetHandle handle)
: base(handle)
        {
        }
    }
}
