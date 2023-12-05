namespace AdventOfCode
{
    public static class Problem1
    {
        public static int SolveA(List<string> input)
        {
            int result = 0;
            foreach(var line in input)
            {
                var first = int.Parse(line.First(c => char.IsDigit(c)).ToString());
                var last = int.Parse(line.Last(c => char.IsDigit(c)).ToString());
                var number = ((first * 10) + last);
                result += number;
                Console.WriteLine(line + " : " + first +"+"+ last + "=" + number);
            }
            Console.WriteLine(result);
            return result;
        }

        public static int SolveB(List<string> input)
        {
            for(int i =0; i<input.Count; i++)
            {
                input[i] = input[i].Replace("one", "one1one", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("two", "two2two", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("three", "three3three", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("four", "four4four", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("five", "five5five", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("six", "six6six", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("seven", "seven7seven", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("eight", "eight8eight", StringComparison.InvariantCultureIgnoreCase);
                input[i] = input[i].Replace("nine", "nine9nine", StringComparison.InvariantCultureIgnoreCase);
            }

            return SolveA(input);
        }
    }
}
