using Grpc.Core;

namespace Rpc.Errors
{
	public static class Error
	{
		public static bool IsEqualErrorCode(StatusCode actual, Rpc.Errors.ErrorCode expected)
		{
			return (ErrorCode)actual == (expected + 10000);
		}
	}
}
