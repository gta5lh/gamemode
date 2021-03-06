// <copyright file="GangUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    public static class GangUtil
    {
        public const string Ballas = "Ballas";
        public const string Bloods = "Bloods";
        public const string Marabunta = "Marabunta";
        public const string TheFamilies = "TheFamilies";
        public const string Vagos = "Vagos";

        public static string ChatColorFromGangName(string gangName)
        {
            switch (gangName)
            {
                case Ballas:
                    return "~p~";

                case Bloods:
                    return "~q~";

                case Marabunta:
                    return "~b~";

                case TheFamilies:
                    return "~g~";

                case Vagos:
                    return "~y~";
            }

            return string.Empty;
        }
    }
}
