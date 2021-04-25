// <copyright file="Gang.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public abstract class Gang
    {
        public string Name { get; set; }

        public Color Color { get; set; }

        public byte BlipColor { get; set; }

        public Marker ItemMarker { get; set; }

        public Marker CarMarker { get; set; }

        public Spawn PlayerSpawn { get; set; }

        public Npc Npc { get; set; }

        public Spawn CarSpawn { get; set; }

        public void Create()
        {
            this.CarMarker.Create();
            this.ItemMarker.Create();
            this.Npc.Create();
            NAPI.Blip.CreateBlip(543, this.PlayerSpawn.Position, 1, this.BlipColor, this.Name, 255, 0, true);
        }
    }
}
