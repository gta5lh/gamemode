﻿// <copyright file="GangUtil.cs" company="lbyte00">
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

        public static readonly Dictionary<byte, long> RewardByRank = new Dictionary<byte, long>()
        {
            { 1, 10},
            { 2, 20},
            { 3, 30},
            { 4, 40},
            { 5, 50},
            { 6, 60},
            { 7, 70},
            { 8, 80},
            { 9, 90},
            { 10, 100},
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
