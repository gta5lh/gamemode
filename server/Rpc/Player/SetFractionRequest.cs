// <copyright file="SetFractionRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System;

	public partial class SetFractionRequest
	{
		public SetFractionRequest(string staticID, long fractionID, long tier, Guid setByID)
		{
			this.StaticID = staticID;
			this.Fraction = fractionID;
			this.Tier = tier;
			this.SetByID = setByID.ToString();
		}
	}
}
