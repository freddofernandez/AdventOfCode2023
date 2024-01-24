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
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var result = Problem5.SolveB(list);
            watch.Stop();

            Console.WriteLine("Result:" + result);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            //var dummyResult = Problem5.SolveB(dummy);
            //Console.WriteLine("Dummy:" + dummyResult);
            Console.ReadLine();
        }

        private static List<string> ReadFileAsStringList(string path)
        {
            var list = new List<string>();
            var input = File.ReadAllText(path);
            input = input.Replace("\r", "\n");
            list.AddRange(input.Split("\n"));
            return list;
        }
    }
}
