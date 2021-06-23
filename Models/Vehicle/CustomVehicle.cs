namespace Gamemode.Models.Vehicle
{
	using GTANetworkAPI;

	public class CustomVehicle : Vehicle
	{
		public ushort OwnerPlayerId { get; set; }

		public CustomVehicle(NetHandle handle)
: base(handle)
		{
		}
	}
}
