using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class RegexService
    {
        public static List<int> GetNumbers(string input)
        {
            string pattern = @"\d+";
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(input);

            List<int> result = new();

            foreach (Match match in matches)
            {
                result.Add(int.Parse(match.Value));
            }

            return result;
        }

        public static List<long> GetLongNumbers(string input)
        {
            string pattern = @"\d+";
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(input);

            List<long> result = new();

            foreach (Match match in matches)
            {
                result.Add(long.Parse(match.Value));
            }

            return result;
        }

        public static Match SingleMatchPattern(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.Match(input);
        }


    }
}
