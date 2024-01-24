using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static partial class RegexService
    {
        public static List<int> GetNumbers(string input)
        {
            var matches = RegexForNumbers().Matches(input);

            List<int> result = new();

            foreach (Match match in matches)
            {
                result.Add(int.Parse(match.Value));
            }

            return result;
        }

        public static List<long> GetLongNumbers(string input)
        {
            var matches = RegexForNumbers().Matches(input);
            List<long> result = [];
            foreach (Match match in matches)
            {
                result.Add(long.Parse(match.Value));
            }

            return result;
        }

        public static Match SingleMatchPattern(string input, string pattern)
        {
            var regex = new Regex(pattern);
            return regex.Match(input);
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex RegexForNumbers();
    }
}
