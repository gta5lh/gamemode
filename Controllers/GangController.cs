// <copyright file="GangController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class GangController : Script
    {
        private readonly Ballas ballas;
        private readonly Bloods bloods;
        private readonly Marabunta marabunta;
        private readonly TheFamilies theFamilies;
        private readonly Vagos vagos;

        public GangController()
        {
            this.ballas = new Ballas();
            this.bloods = new Bloods();
            this.marabunta = new Marabunta();
            this.theFamilies = new TheFamilies();
            this.vagos = new Vagos();
        }

        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.ballas.Create();
            this.bloods.Create();
            this.marabunta.Create();
            this.theFamilies.Create();
            this.vagos.Create();
        }
    }
}
