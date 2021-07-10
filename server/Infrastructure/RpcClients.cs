// <copyright file="ResourceStartController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Grpc.Net.Client;

namespace Gamemode.Infrastructure
{
	public static class RpcClients
	{
		public static Rpc.Zone.ZoneService.ZoneServiceClient ZoneService { get; }
		public static Rpc.Report.ReportService.ReportServiceClient ReportService { get; }
		public static Rpc.GangWar.GangWarService.GangWarServiceClient GangWarService { get; }
		public static Rpc.User.UserService.UserServiceClient UserService { get; }
		public static Rpc.GameServer.GameServerService.GameServerServiceClient GameServerService { get; }

		static RpcClients()
		{
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

			string? apiCertificate = System.Environment.GetEnvironmentVariable("API_CERTIFICATE");
			if (apiCertificate == null)
			{
				apiCertificate = "-----BEGIN CERTIFICATE-----\nMIID0TCCArmgAwIBAgIUXSwoJC8AP1qD0TPVCdx/eqOmLBIwDQYJKoZIhvcNAQEL\nBQAweDELMAkGA1UEBhMCRUUxEDAOBgNVBAgMB0VzdG9uaWExEDAOBgNVBAcMB1Rh\nbGxpbm4xDzANBgNVBAoMBmd0YTVsaDESMBAGA1UEAwwJbG9jYWxob3N0MSAwHgYJ\nKoZIhvcNAQkBFhFzdXBwb3J0QGd0YTVsaC5ydTAeFw0yMTA3MDkxMzEwMDhaFw0z\nMTA3MDcxMzEwMDhaMHgxCzAJBgNVBAYTAkVFMRAwDgYDVQQIDAdFc3RvbmlhMRAw\nDgYDVQQHDAdUYWxsaW5uMQ8wDQYDVQQKDAZndGE1bGgxEjAQBgNVBAMMCWxvY2Fs\naG9zdDEgMB4GCSqGSIb3DQEJARYRc3VwcG9ydEBndGE1bGgucnUwggEiMA0GCSqG\nSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDFKwD2jhjPitSX5gdvheEji0yY7PuM1TcB\n/11fe+2B3+wkqTGfoaiEA/kDFfXtzKgIN414czSthRBZg5O5EpXO7tu94aAfLk5c\n4XnNXuz8fPS3Iz3HIPUDwbToXyoRJJ+ZrQFfdAtPrhMbGB/A8fd7Ku8qYG2l8B8/\nrzjy3jCIMpMRzZL48o1v67JmhLyV5IcYjCtQjKmXozJm4sRnhVbmVHrOsrXcgv9u\n33uQEh2qS0mfF6pIoiaOIBaegF3tHMHCWRCpR6tmVKjDMtwOZIP1QODyC800+xgU\n9BHzHrdtoXvpAVrgOuJ7XSX5S3vaJK/O8lWAhEksefqUtaMlOPjvAgMBAAGjUzBR\nMB0GA1UdDgQWBBRwtPDxBYVxGJE8fftBKlMggAl6RzAfBgNVHSMEGDAWgBRwtPDx\nBYVxGJE8fftBKlMggAl6RzAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3DQEBCwUA\nA4IBAQCbgaRdq5yoV8WKe+MnuRQxjmqfoIx4WXz237rLioVralHE5/nfNBgKPE8i\niZPpNaZopjeUBZ4ZYFoNnDN8HPWLiUoZbRz9hZ3NsUWwYYzeZBm2AVVk6DzUfY+W\nNZ28205QM9uaq7+lASJUCKhgXhJHEpZFgSD8x/uMJ0a1wsy2Waj/4UXLrn6ULcAU\nnaMjNHYoiqD+2/Sr42uja//x9bygVaw7zbsKcrKtGajGahCMnXb8CQkmSoKzM28C\n+IMBxXO0x9yTOXM7Wce6R/hrHc0t7XgHhkqILy1KieCkVgHpboocog5KWJlFn+rI\nLNooycfad/wqn4rKPtSQVQVJSLlY\n-----END CERTIFICATE-----";
			}

			string? apiURL = System.Environment.GetEnvironmentVariable("API_URL");
			if (apiURL == null)
			{
				apiURL = "https://localhost:8000/";
			}

			handler.ClientCertificates.Add(new X509Certificate2(System.Text.Encoding.UTF8.GetBytes(apiCertificate)));

			var channel = GrpcChannel.ForAddress(apiURL, new GrpcChannelOptions
			{
				HttpHandler = handler
			});

			ZoneService = new Rpc.Zone.ZoneService.ZoneServiceClient(channel);
			ReportService = new Rpc.Report.ReportService.ReportServiceClient(channel);
			GangWarService = new Rpc.GangWar.GangWarService.GangWarServiceClient(channel);
			UserService = new Rpc.User.UserService.UserServiceClient(channel);
			GameServerService = new Rpc.GameServer.GameServerService.GameServerServiceClient(channel);
		}
	}
}
