// <copyright file="RpcClients.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Infrastructure
{
	using System;
	using System.Net.Http;
	using System.Net.Security;
	using System.Security.Cryptography.X509Certificates;
	using Gamemode.Game.ServerSettings;
	using Grpc.Core;
	using Grpc.Net.Client;
	using Grpc.Net.Client.Configuration;
	using Microsoft.Extensions.Logging;
	using NLog.Extensions.Logging;

	public static class RpcClients
	{
		static RpcClients()
		{
			SocketsHttpHandler handler = new()
			{
				PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
				KeepAlivePingDelay = TimeSpan.FromSeconds(10),
				KeepAlivePingTimeout = TimeSpan.FromSeconds(20),
				KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always,
				EnableMultipleHttp2Connections = true,
				SslOptions = new SslClientAuthenticationOptions()
				{
					ClientCertificates = new X509CertificateCollection(),
				},
			};

			string cert = Settings.PlatformCertificate.Replace("-----BEGIN CERTIFICATE-----", string.Empty).Replace("-----END CERTIFICATE-----", string.Empty).Replace("\\n", Environment.NewLine);

			handler.SslOptions.ClientCertificates.Add(new X509Certificate2(Convert.FromBase64String(cert)));
			handler.SslOptions.RemoteCertificateValidationCallback = (message, certificate2, arg3, arg4) => true;

			var defaultmethodConfig = new MethodConfig
			{
				Names = { MethodName.Default },
				RetryPolicy = new RetryPolicy
				{
					MaxAttempts = 5,
					InitialBackoff = TimeSpan.FromSeconds(1),
					MaxBackoff = TimeSpan.FromSeconds(5),
					BackoffMultiplier = 1.5,
					RetryableStatusCodes = { StatusCode.Unavailable },
				},
			};

			const int mb64 = 64 * 1024 * 1024;

			var channel = GrpcChannel.ForAddress(Settings.PlatformURL, new GrpcChannelOptions
			{
				HttpHandler = handler,
				ServiceConfig = new ServiceConfig { MethodConfigs = { defaultmethodConfig } },
				MaxReceiveMessageSize = mb64,
				MaxSendMessageSize = mb64,
				MaxRetryBufferSize = mb64,
				LoggerFactory = LoggerFactory.Create(logging => logging.AddNLog()),
			});

			ZoneService = new Rpc.Zone.ZoneService.ZoneServiceClient(channel);
			ReportService = new Rpc.Report.ReportService.ReportServiceClient(channel);
			GangWarService = new Rpc.GangWar.GangWarService.GangWarServiceClient(channel);
			PlayerService = new Rpc.Player.PlayerService.PlayerServiceClient(channel);
			GameServerService = new Rpc.GameServer.GameServerService.GameServerServiceClient(channel);
		}

		public static Rpc.Zone.ZoneService.ZoneServiceClient ZoneService { get; }

		public static Rpc.Report.ReportService.ReportServiceClient ReportService { get; }

		public static Rpc.GangWar.GangWarService.GangWarServiceClient GangWarService { get; }

		public static Rpc.Player.PlayerService.PlayerServiceClient PlayerService { get; }

		public static Rpc.GameServer.GameServerService.GameServerServiceClient GameServerService { get; }
	}
}
