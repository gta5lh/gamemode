// <copyright file="Eventbus.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Eventbus
{
	using EasyNetQ;
	using EasyNetQ.AutoSubscribe;

	[Queue("deposit.made", ExchangeName = "eventbus")]
	public partial class DepositMadeEvent
	{
	}
}
