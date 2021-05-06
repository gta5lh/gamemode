// <copyright file="GangUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using System.Collections.Generic;
using GTANetworkAPI;

namespace Gamemode
{
    public static class GangUtil
    {
        public static readonly Dictionary<string, Color> GangColorByName = new Dictionary<string, Color>()
        {
            { "ballas", Ballas.Color},
            { "bloods", Bloods.Color},
            { "marabunta", Marabunta.Color},
            { "the_families", TheFamilies.Color},
            { "vagos", Vagos.Color},
        };

        public const string BallasName = "Ballas";
        public const string BloodsName = "Bloods";
        public const string MarabuntaName = "Marabunta";
        public const string TheFamiliesName = "TheFamilies";
        public const string VagosName = "Vagos";

        public static string ChatColorFromGangName(string gangName)
        {
            switch (gangName)
            {
                case BallasName:
                    return "~p~";

                case BloodsName:
                    return "~q~";

                case MarabuntaName:
                    return "~b~";

                case TheFamiliesName:
                    return "~g~";

                case VagosName:
                    return "~y~";
            }

            return string.Empty;
        }
    }
}
