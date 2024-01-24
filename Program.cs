using AdventOfCode2023;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please insert your input file inside Input.txt! ");
            var list = ReadFileAsStringList("Input.txt");
            var dummy = ReadFileAsStringList("DummyInput.txt");

            //Call the Problem and part you want to execute!
            var result = Problem5.SolveB(list);
            var dummyResult = Problem5.SolveB(dummy);
            Console.WriteLine("Result:" + result);
            Console.WriteLine("Dummy:" + dummyResult);
            Console.ReadLine();
        }

        private static List<string> ReadFileAsStringList(string path)
        {
            var list = new List<string>();
            var input = File.ReadAllText(path);
            list.AddRange(input.Split("\n"));
            return list;
        }
    }
}
