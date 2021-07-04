namespace GamemodeClient.Models
{
	public class Zone
	{
		public short Id { get; set; }

		public short X { get; set; }

		public short Y { get; set; }

		public byte FractionId { get; set; }

		public bool Battleworthy { get; set; }

		public byte BlipColor { get; set; }

		public int? BlipId { get; set; }

		public bool IsWarInProgress { get; set; }
	}
}
