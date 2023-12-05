using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Problem2
    {
        public static int SolveA(List<string> input)
        {
            int result = 0;
            foreach (string s in input)
            {
                Game game = new Game(s);
                if (game.IsPossible)
                {
                    result += game.Id;
                }

            }
            Console.WriteLine(result);
            return result;
        }

        public static int SolveB(List<string> input)
        {
            int result = 0;
            foreach (string s in input)
            {
                Game game = new Game(s);
                result += game.Power;

            }
            Console.WriteLine(result);
            return result;
        }

        public class Game
        {
            public int Id { get; set; }
            public int GreenMaxCount { get; set; } = 0;
            public int RedMaxCount { get; set; } = 0;
            public int BlueMaxCount { get; set; } = 0;
            public bool IsPossible => (GreenMaxCount <= 13 && RedMaxCount <= 12 && BlueMaxCount <= 14);

            public int Power => (GreenMaxCount * RedMaxCount * BlueMaxCount);

            public Game(string line) 
            {
                string pattern = @"Game (\d+):";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(line);
                Id = int.Parse(match.Groups[1].Value);

                var gameList = line.Split(':').ToList().Last().Split(";").ToList();
                foreach(var game in gameList)
                {
                    string bluePattern = @"(\d+) blue";
                    Regex blueRegex = new Regex(bluePattern);
                    Match blueMatch = blueRegex.Match(game);
                    if (blueMatch.Success)
                    {
                        int blueValue = int.Parse(blueMatch.Groups[1].Value);
                        BlueMaxCount = BlueMaxCount < blueValue ? blueValue : BlueMaxCount;
                    }
                    string greenPattern = @"(\d+) green";
                    Regex greenRegex = new Regex(greenPattern);
                    Match greenMatch = greenRegex.Match(game);
                    if (greenMatch.Success)
                    {
                        int greenValue = int.Parse(greenMatch.Groups[1].Value);
                        GreenMaxCount = GreenMaxCount < greenValue ? greenValue : GreenMaxCount;
                    }
                    string redPattern = @"(\d+) red";
                    Regex redRegex = new Regex(redPattern);
                    Match redMatch = redRegex.Match(game);
                    if (redMatch.Success)
                    {
                        int redValue = int.Parse(redMatch.Groups[1].Value);
                        RedMaxCount = RedMaxCount < redValue ? redValue : RedMaxCount;
                    }
                }
                

            }
        }
    }
}
