// <copyright file="Marabunta.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Colshape;
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class Marabunta : Gang
    {
        public static readonly Spawn Spawn = new Spawn(new Vector3(1378.1, -1518.1, 57.79), 147.75f);
        public static readonly Color Color = new Color(0, 118, 215);

        public Marabunta()
        {
            this.Name = "Marabunta";
            this.GangColor = Color;
            this.PlayerSpawn = Spawn;
            this.CarMarker = new Marker(new Vector3(1371.9, -1519.2, 57.52), this.GangColor, (MarkerType)36, "Car", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(1374.1, -1523.4, 57), 174.37f);
            this.ItemMarker = new Marker(new Vector3(1384.2, -1521.7, 57.53), this.GangColor, (MarkerType)41, "Weapon", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(1367, -1527, 56.7), -89.8f, "Старший", PedHash.SalvaGoon01GMY, new Colshape.GangNpcEvent(NpcUtil.NpcMarabunta));
            this.BlipColor = 67;
        }
    }
}
