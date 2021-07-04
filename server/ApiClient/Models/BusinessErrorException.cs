using System;

namespace Gamemode.ApiClient.Models
{
	public class BusinessErrorException : Exception
	{

		public BusinessErrorException(string message)
		   : base(message)
		{
		}
	}
}
