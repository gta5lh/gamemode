// <copyright file="CPlayer.Fraction.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Fraction;
	using Gamemode.Game.Gang;
	using GTANetworkAPI;
	using Rpc.Player;

	public partial class CPlayer
	{
		private long? fractionValue;

		private string? fractionRankName;

		public string? FractionRankName
		{
			get => this.fractionRankName;
			set
			{
				this.fractionRankName = value;
				this.TriggerEvent("FractionRankNameUpdated", this.fractionRankName);
			}
		}

		public long? Fraction
		{
			get => this.fractionValue;

			set
			{
				if (this.fractionValue != null)
				{
					Cache.UnloadFractionMemberFromCache((byte)this.fractionValue, this.StaticId);
				}

				this.fractionValue = value;

				if (this.fractionValue != null)
				{
					this.TriggerEvent("FractionNameUpdated", Util.GangReadableNameById[(byte)this.fractionValue]);
					Cache.LoadFractionMemberToCache((byte)this.fractionValue, this.StaticId, this.Name);
					this.SetBlipColor(Util.BlipColorByGangId[this.fractionValue.Value]);
					return;
				}

				this.SetBlipColor(62);
				this.TriggerEvent("FractionNameUpdated");
			}
		}

		public long? FractionRank { get; set; }

		public async Task RankUp()
		{
			if (this.FractionRank >= 10)
			{
				return;
			}

			long fractionRank = (long)(this.FractionRank + 1);
			SetFractionResponse setFractionResponse;

			try
			{
				SetFractionRequest setFractionRequest = new(this.StaticId, this.Fraction.Value, fractionRank, this.PKId);

				setFractionResponse = await Infrastructure.RpcClients.PlayerService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			NAPI.Task.Run(() =>
			{
				this.FractionRank = fractionRank;
				this.CurrentExperience = (short)(this.CurrentExperience - this.RequiredExperience.Value);
				this.RequiredExperience = setFractionResponse.TierRequiredExperience;
				this.FractionRankName = setFractionResponse.TierName;
			});
		}

		public async Task RankDown()
		{
			if (this.FractionRank <= 1)
			{
				return;
			}

			byte fractionRank = (byte)(this.FractionRank - 1);
			SetFractionResponse setFractionResponse;

			try
			{
				SetFractionRequest setFractionRequest = new(this.StaticId, this.Fraction.Value, fractionRank, this.PKId);

				setFractionResponse = await Infrastructure.RpcClients.PlayerService.SetFractionAsync(setFractionRequest);
			}
			catch (Exception)
			{
				return;
			}

			NAPI.Task.Run(() =>
			{
				this.FractionRank = fractionRank;
				this.CurrentExperience = (short)(setFractionResponse.TierRequiredExperience - 1);
				this.RequiredExperience = setFractionResponse.TierRequiredExperience;
				this.FractionRankName = setFractionResponse.TierName;
			});
		}
	}
}
