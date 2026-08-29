using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rikoo.Framework.Core.Utilities
{
    public static class StringExtension
    {
        public static List<string> SplitWords(this string input)
        {
            var matches = Regex.Matches(input, @"([A-Z]+(?=[A-Z][a-z])|[A-Z][a-z]+)");

            var result = new List<string>();

            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }

            return result;
        }
    }
}