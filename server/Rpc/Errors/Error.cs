// <copyright file="Error.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Rpc.Errors
{
	using Grpc.Core;

	public static class Error
	{
		public static bool IsEqualErrorCode(StatusCode actual, Rpc.Errors.ErrorCode expected)
		{
			return (ErrorCode)actual == (expected + 10000);
		}
	}
}
