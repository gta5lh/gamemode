// <copyright file="TheFamilies.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Colshape;
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class TheFamilies : Gang
    {
        public static readonly Spawn Spawn = new Spawn(new Vector3(-14, -1446, 30.65), 179);
        public static readonly Color Color = new Color(0, 255, 0);

        public TheFamilies()
        {
            this.Name = "The Families";
            this.GangColor = Color;
            this.PlayerSpawn = Spawn;
            this.CarMarker = new Marker(new Vector3(-25, -1433, 30.65), this.GangColor, (MarkerType)36, "Car", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(-24, -1436, 30.65), -179f);
            this.ItemMarker = new Marker(new Vector3(-10, -1445, 30.75), this.GangColor, (MarkerType)41, "Weapon", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(-18, -1448, 30.65), -48, "Старший", PedHash.Stretch, new Colshape.GangNpcEvent(NpcUtil.NpcNameTheFamilies, GangUtil.NpcIdTheFamilies));
            this.BlipColor = 2;
        }
    }
}
