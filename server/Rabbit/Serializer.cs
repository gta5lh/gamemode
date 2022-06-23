using System;
using System.Collections.Concurrent;
using EasyNetQ;

namespace server.Rabbit
{
	public class Serializer : ITypeNameSerializer
	{
		public Serializer()
		{
		}

		/// <inheritdoc />
		public string Serialize(Type type)
		{
			return type.ToString();
		}

		/// <inheritdoc />
		public Type? DeSerialize(string typeName)
		{
			switch (typeName)
			{
				case "DepositMadeEvent":
					return typeof(Rpc.Eventbus.DepositMadeEvent);
			}

			return null;
		}
	}
}
