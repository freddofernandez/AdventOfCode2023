using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Problem5
    {
        public static long SolveA(List<string> input)
        {
            Almanac almanac = new(input[0]);
            input.RemoveAt(0);
            foreach(var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (RegexService.SingleMatchPattern(line, @":").Success)
                {
                    almanac.AddConversionTable();
                }
                else
                {
                    almanac.UpdateLatestConversion(line);
                }
            }

            var result = almanac.GetLowestLocationForSeeds();

            return result;
        }
        
        public static long SolveB(List<string> input)
        {
            Almanac almanac = new(input[0]);
            input.RemoveAt(0);
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (RegexService.SingleMatchPattern(line, @":").Success)
                {
                    almanac.AddConversionTable();
                }
                else
                {
                    almanac.UpdateLatestConversion(line);
                }
            }

            almanac.ReinterpretSeeds();
            var result = almanac.GetLowestLocationForSeeds();

            return result;
        }
    }
    
    public class Almanac(string seedsInput)
    {
        public List<long> Seeds { get; set; } = RegexService.GetLongNumbers(seedsInput);
        public List<ConversionTable> ConversionTables { get; set; } = [];

        public void ReinterpretSeeds()
        {
            List<long> newSeeds = new List<long>();
            for(int i = 0; i < Seeds.Count; i+=2)
            {
                for(long j = 0; j < Seeds[i+1]; j++)
                {
                    newSeeds.Add(Seeds[i] + j);
                }
            }
            Seeds = newSeeds;
        }

        public void AddConversionTable()
        {
            ConversionTables.Add(new ConversionTable(){ });
        }

        public void UpdateLatestConversion(string input)
        {
            ConversionTables.Last().Maps.Add(new(input));
        }

        public long GetLocationForSeed(long seed)
        {
            long tracker = seed;
            //Console.WriteLine("Seed: " + seed);
            var counter = 0;
            foreach(var conversionTable in ConversionTables)
            {
                //Console.WriteLine($"ConversionTable {counter} --------------------");
                var map = conversionTable.Maps.FirstOrDefault(m => tracker >= m.Source && tracker <= m.Source + m.Range);
                if (map != null)
                {
                    tracker = tracker + (map.Destination - map.Source);
                    //Console.WriteLine($"Found match for source {map.Source}, and destination {map.Destination} with range {map.Range} --> {tracker}");
                } 
                else
                {
                    //Console.WriteLine("Not match found, will continue as " + tracker);
                    continue;
                }
                counter++;
            }
            return tracker;
        }

        public long GetLowestLocationForSeeds()
        {
            long lowerLocation = long.MaxValue;

            foreach(var seed in Seeds)
            {
                long location = GetLocationForSeed(seed);
                lowerLocation = location < lowerLocation ? location : lowerLocation;
            }

            return lowerLocation;
        }
    }

    public class ConversionTable
    {
        public List<Map> Maps { get; set; } = [];
    }

    public class Map
    {
        public long Source { get; set; }
        public long Destination { get; set; }
        public long Range { get; set; }

        public Map(string inputLine)
        {
            var list = RegexService.GetLongNumbers(inputLine);
            Destination = list[0];
            Source = list[1];
            Range = list[2];
        }
    }
}
