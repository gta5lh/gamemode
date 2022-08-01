// <copyright file="SaveAllRequest.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Player
{
	using System.Collections.Generic;

	public partial class SaveAllRequest
	{
		public SaveAllRequest(List<SaveRequest> saveRequests)
		{
			this.SaveRequests.Add(saveRequests);
		}
	}
}
