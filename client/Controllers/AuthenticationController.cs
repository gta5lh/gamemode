// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using System.Collections.Generic;
	using GamemodeCommon.Authentication.Models;
	using Newtonsoft.Json;
	using RAGE;
	using RAGE.Ui;

	public class AuthenticationController : Events.Script
	{
		private const string AuthenticationPath = "package://cs_packages/gamemode/Frontend/Authentication/index.html";
		private readonly HtmlWindow loginCEF;

		public AuthenticationController()
		{
			RAGE.Input.Bind(VirtualKeys.OEM3, false, this.OnCursorKeyPressed);

			this.loginCEF = new HtmlWindow(AuthenticationPath);
			Ui.OpenUI(this.loginCEF);
			Task.Run(() => Cursor.Visible = true, 1000);

			Events.Add("LoginSubmitted", this.OnLoginSubmitted);
			Events.Add("LoginSubmittedFailed", this.OnLoginSubmittedFailed);

			Events.Add("RegisterSubmitted", this.OnRegisterSubmitted);
			Events.Add("RegisterSubmittedFailed", this.OnRegisterSubmittedFailed);

			Events.Add("LogIn", this.OnLogIn);

			Events.Add("WaitAuthenticationAction", this.OnWaitAuthenticationAction);
		}

		private void OnCursorKeyPressed()
		{
			if (!Player.AuthenticationScreen)
			{
				return;
			}

			Cursor.Visible = true;
		}

		private void OnWaitAuthenticationAction(object[] request)
		{
			this.loginCEF.ExecuteJs("wait()");
		}

		private void OnLoginSubmitted(object[] request)
		{
			LoginRequest loginRequest = JsonConvert.DeserializeObject<LoginRequest>((string)request[0]);
			List<string> invalidFieldNames = loginRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				this.loginCEF.ExecuteJs($"loginFailed({JsonConvert.SerializeObject(invalidFieldNames)})");
				return;
			}

			Events.CallRemote("LoginSubmitted", (string)request[0]);
		}

		private void OnLoginSubmittedFailed(object[] args)
		{
			string invalidFieldNames = (string)args[0];

			this.loginCEF.ExecuteJs($"loginFailed({invalidFieldNames})");
		}

		private void OnRegisterSubmitted(object[] request)
		{
			RegisterRequest registerRequest = JsonConvert.DeserializeObject<RegisterRequest>((string)request[0]);
			List<string> invalidFieldNames = registerRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				this.loginCEF.ExecuteJs($"registerFailed({JsonConvert.SerializeObject(invalidFieldNames)})");
				return;
			}

			Events.CallRemote("RegisterSubmitted", (string)request[0]);
		}

		private void OnRegisterSubmittedFailed(object[] args)
		{
			string invalidFieldNames = (string)args[0];

			this.loginCEF.ExecuteJs($"registerFailed({invalidFieldNames})");
		}

		private void OnLogIn(object[] args)
		{
			this.loginCEF.ExecuteJs("logIn()");
			Task.Run(
				() =>
				{
					Ui.CloseUI(this.loginCEF);
					Player.AuthenticationScreen = false;
				}, 550);
		}
	}
}
