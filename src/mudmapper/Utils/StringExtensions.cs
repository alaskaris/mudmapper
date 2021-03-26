using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mudmapper.Utils
{
    internal static class StringExtensions
    {
        public static string reverse(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        public static bool isCommand(this string input)
        {
            HashSet<string> directions = new HashSet<string>() { "n", "N", "s", "S", "e", "E", "w", "W", "u", "U", "d", "D" };
            if (!String.IsNullOrWhiteSpace(input) && input.Length == 1 && directions.Contains(input))
            {
                return true;
            }
            return false;
        }

        public static (string, string) getExitsCommand(this string input)
        {
            Regex soloPattern = new Regex(@"Exits:([NESWUD]+)> (.*)");
            Match match = soloPattern.Match(input);
            if (match.Success)
            {
                return (match.Groups[1].Value, match.Groups[2].Value);
            }
            return (null, null);
        }
    }
}
