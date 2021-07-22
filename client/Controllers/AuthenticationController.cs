// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using GamemodeCommon.Authentication.Models;
	using static GamemodeClient.Controllers.Cef.Cef;
	using Newtonsoft.Json;
	using RAGE;
	using RAGE.Ui;
	using System;

	public class AuthenticationController : Events.Script
	{
		public AuthenticationController()
		{
			RAGE.Input.Bind(VirtualKeys.OEM3, false, this.OnCursorKeyPressed);

			ShowAuth();
			Task.Run(() => Cursor.Visible = true, 1000);

			Events.Add("LoginSubmitted", this.OnLoginSubmitted);
			Events.Add("RegisterSubmitted", this.OnRegisterSubmitted);
		}

		private void OnCursorKeyPressed()
		{
			if (!Player.AuthenticationScreen)
			{
				return;
			}

			Cursor.Visible = true;
		}

		private async void OnLoginSubmitted(object[] request)
		{
			LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>((string)request[0]);
			List<string> invalidFieldNames = loginRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				LoginFailed(JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			try
			{
				string result = (string)await Events.CallRemoteProc("LoginSubmitted", (string)request[0]);
				if (result != "")
				{
					LoginFailed(result);
					return;
				}

				LogIn();
			}
			catch { }
		}

		private async void OnRegisterSubmitted(object[] request)
		{
			RegisterRequest registerRequest = JsonConvert.DeserializeObject<RegisterRequest>((string)request[0]);
			List<string> invalidFieldNames = registerRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				RegisterFailed(JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			try
			{
				string result = (string)await Events.CallRemoteProc("RegisterSubmitted", (string)request[0]);
				if (result != "")
				{
					RegisterFailed(result);
					return;
				}

				LogIn();
			}
			catch { }
		}

		private void LogIn()
		{
			HideAuth();
			Player.AuthenticationScreen = false;
		}
	}
}
