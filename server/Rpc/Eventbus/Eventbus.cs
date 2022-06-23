namespace Rpc.Eventbus
{
	using EasyNetQ;

	[Queue("deposit.made", ExchangeName = "eventbus")]
	public partial class DepositMadeEvent
	{
		public DepositMadeEvent(long amount, long playerID)
		{
			this.Amount = amount;
			this.PlayerID = playerID;
		}
	}
}
