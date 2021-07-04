// <copyright file="FreezeController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using RAGE;

	public class FreezeController : Events.Script
	{
		public FreezeController()
		{
			Events.Add("FreezePlayer", this.OnFreezePlayer);
		}

		private void OnFreezePlayer(object[] args)
		{
			bool freeze = (bool)args[0];
			Player.CurrentPlayer.FreezePosition(freeze);
		}
	}
}
