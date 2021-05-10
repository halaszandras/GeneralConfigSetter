using System;
using System.Collections.Generic;

namespace ACrypto
{
    public static class Configuration
    {
        public const int MAX_LENGTH = 5;
        public const char SEPARATOR = 'p';
        public const char BINDER = 'r';
        internal static readonly char[] SPECIAL_CHARACTERS = new[] {'/', '-', '.'};
        internal static readonly char[] REGULAR_CHARACTERS = new[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'

        };
        public static readonly string KEY_LOCATION = $"{AppDomain.CurrentDomain.BaseDirectory}Resources\\PonySimulator.txt";

        public static List<char> CHARACTERS {
            get
            {
                char[] characters = new char[REGULAR_CHARACTERS.Length + SPECIAL_CHARACTERS.Length];
                REGULAR_CHARACTERS.CopyTo(characters,0);
                SPECIAL_CHARACTERS.CopyTo(characters, REGULAR_CHARACTERS.Length);
                return new List<char>(characters);
            }
        }
    }
}
