using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Problem3
    {
        public static int SolveA(List<string> input)
        {
            List<Coordinates> SignCoordinates = new List<Coordinates>();
            List<Number> Numbers = new List<Number>();

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
                string pattern = @"\d+";

                Regex regex = new Regex(pattern);
                var matches = regex.Matches(line);

                foreach(Match match in matches)
                {
                    Numbers.Add(new Number(new Coordinates(match.Index, i), int.Parse(match.Value)));
                }

                pattern = "[^a-zA-Z0-9.]";
                Regex symbolRegex = new Regex(pattern);
                matches = symbolRegex.Matches(line);

                foreach (Match match in matches)
                {
                    SignCoordinates.Add(new Coordinates(match.Index, i));
                }
            }

            int result = 0;

            foreach(Number num in Numbers)
            {
                if (num.Coordinates.Any(c => SignCoordinates.Any(s => s.IsAdjacent(c))))
                {
                    num.IsAjacentToSign = true;
                    result += num.Value;
                }
                
            }

            return result;
        }

        public static int SolveB(List<string> input)
        {
            List<Coordinates> SignCoordinates = new List<Coordinates>();
            List<Number> Numbers = new List<Number>();

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
                string pattern = @"\d+";

                Regex regex = new Regex(pattern);
                var matches = regex.Matches(line);

                foreach (Match match in matches)
                {
                    Numbers.Add(new Number(new Coordinates(match.Index, i), int.Parse(match.Value)));
                }

                pattern = "[^a-zA-Z0-9.]";
                Regex symbolRegex = new Regex(pattern);
                matches = symbolRegex.Matches(line);

                foreach (Match match in matches)
                {
                    SignCoordinates.Add(new Coordinates(match.Index, i));
                }
            }

            int result = 0;

            foreach (Coordinates sign in SignCoordinates)
            {
                List<Number> adjacentNums = Numbers.Where(n => n.Coordinates.Any(c => c.IsAdjacent(sign))).ToList();

                if(adjacentNums.Count() == 2)
                {
                    result += adjacentNums[0].Value * adjacentNums[1].Value;
                }

            }

            return result;
        }



        public class Coordinates
        {
            public int X { get; set; }
            
            public int Y { get; set; }

            public Coordinates(int x, int y)
            {
                X = x; Y = y;
            }

            public bool IsAdjacent(Coordinates coordinates)
            {
                return (coordinates.X <= X+1 && 
                        coordinates.Y <= Y+1 &&
                        coordinates.X >= X-1 &&
                        coordinates.Y >= Y-1);
            }
        }

        public class Number
        {
            public List<Coordinates> Coordinates { get; set; } = [];
            
            public int Value { get; set; }

            public int Length => Value.ToString().Length;

            public bool IsAjacentToSign { get; set; }

            public Number(Coordinates firstNumberCoordinate, int value)
            {
                Value = value;
                for(int i = 0; i < Length; i++)
                {
                    Coordinates.Add(new(firstNumberCoordinate.X + i, firstNumberCoordinate.Y));
                }
            }

        }
    }
}
