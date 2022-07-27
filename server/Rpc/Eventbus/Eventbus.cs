namespace Rpc.Eventbus
{
	using EasyNetQ;
	using EasyNetQ.AutoSubscribe;

	[Queue("deposit.made", ExchangeName = "eventbus")]
	public partial class DepositMadeEvent
	{

	}
}
