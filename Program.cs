namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please insert your input file inside Input.txt! ");
            var list = ReadFileAsStringList();

            //Call the Problem and part you want to execute!
            Problem3.SolveB(list);

            Console.ReadLine();
        }

        private static List<string> ReadFileAsStringList()
        {
            var list = new List<string>();
            var input = File.ReadAllText(".\\Input.txt");
            list.AddRange(input.Split("\r\n"));
            return list;
        }
    }
}
