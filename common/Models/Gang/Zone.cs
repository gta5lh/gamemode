namespace GamemodeCommon.Models.Gang
{
	public class Zone
	{
		public Zone(long id, long x, long y, long fractionId, bool battleworthy, byte blipColor)
		{
			Id = id;
			X = x;
			Y = y;
			FractionId = fractionId;
			Battleworthy = battleworthy;
			BlipColor = blipColor;
		}

		public long Id { get; set; }

		public long X { get; set; }

		public long Y { get; set; }

		public long FractionId { get; set; }

		public bool Battleworthy { get; set; }

		public byte BlipColor { get; set; }

		public int? BlipId { get; set; }

		public bool IsWarInProgress { get; set; }
	}
}
