// <copyright file="Serializer.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Server.Rabbit
{
	using System;
	using System.Collections.Concurrent;
	using EasyNetQ;

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
