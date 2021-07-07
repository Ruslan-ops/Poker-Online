using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PokerEngineTests")]
[assembly: InternalsVisibleTo("ConsoleUI")]

namespace PokerEngine
{
    internal static class StringLibrary
    {
        public static bool StartsWithUpper(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            char ch = str[0];
            return char.IsUpper(ch);
        }
    }
}
