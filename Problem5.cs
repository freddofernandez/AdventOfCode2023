using System.Collections.Generic;
using System.Data.Common;
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

            //Console.WriteLine("Seeds:");
            //foreach(var seed in almanac.Seeds)
            //{
            //    Console.Write(seed+" ");
            //}

            //int i = 1;
            //foreach(var table in almanac.ConversionTables)
            //{
            //    Console.WriteLine("table " + i);
            //    foreach(var map in table.Maps)
            //    {
            //        Console.WriteLine($"{map.Destination} {map.Source} {map.Range}");
            //    }
            //    i++;
            //}

            almanac.ReinterpretSeedsAsRange();
            var result = almanac.GetLowestLocationForRange();

            return result;
        }
    }
    
    public class Almanac(string seedsInput)
    {
        public List<long> Seeds { get; set; } = RegexService.GetLongNumbers(seedsInput);
        public List<ConversionTable> ConversionTables { get; set; } = [];

        public List<Range> SeedRange { get; set; } = [];

        public void ReinterpretSeedsAsRange()
        {
            for(int i = 0; i < Seeds.Count; i+=2)
            {
                SeedRange.Add(new(Seeds[i], Seeds[i] + Seeds[i + 1] - 1));
            }
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

        public long GetLowestLocationForRange()
        {
            long result = long.MaxValue;
            List<Range> TotalConvertedSeedRange = SeedRange;
            for(int i = 0; i < ConversionTables.Count; i++)
            {
                List<Range> TableConvertedSeedRange = [];
                foreach(var range in TotalConvertedSeedRange)
                {
                    if(ConversionTables[i].Maps.Any(m => Intersect(range, m.SourceRange)))
                    {
                        List<Map> maps = ConversionTables[i].Maps.Where(m => Intersect(range, m.SourceRange)).ToList();
                        TableConvertedSeedRange.AddRange(GetRangesWithIntersection(range, maps));
                        
                    } 
                    else
                    {
                        TableConvertedSeedRange.Add(range);
                    }
                }
                TotalConvertedSeedRange = TableConvertedSeedRange;
            }
            foreach(var range in TotalConvertedSeedRange.OrderBy(x => x.Start))
            {
                Console.WriteLine(range.Start);
                Console.WriteLine(range.End);
                result = range.Start < result ? range.Start : result;
            }
            return result;
        }

        static bool Intersect(Range source, Range modificationRange)
        {
            bool startIsInside = (source.Start >= modificationRange.Start && source.Start <= modificationRange.End);
            bool endIsInside = (source.End >= modificationRange.Start && source.End <= modificationRange.End);

            return startIsInside || endIsInside;
        }

        static List<Range> GetRangesWithIntersection(Range source, List<Map> maps /*Range modificationRange, long incrementForOverlap*/)
        {
            List<Range> overlappedRanges = new List<Range>();
            List<Range> nonOverlappedRanges = new List<Range>();

            foreach (var map in maps)
            {
                Range modificationRange = map.SourceRange;
                long incrementForOverlap = map.Increment;

                bool startIsInside = (source.Start >= modificationRange.Start && source.Start <= modificationRange.End);
                bool endIsInside = (source.End >= modificationRange.Start && source.End <= modificationRange.End);

                //if(!startIsInside && source.Start != modificationRange.Start)
                //{
                //    result.Add(new(source.Start, modificationRange.Start - 1));
                //}

                //if (!endIsInside && source.End != modificationRange.End)
                //{
                //    result.Add(new(modificationRange.End+1, source.End));
                //}

                long overlapStart = startIsInside ? source.Start : modificationRange.Start;
                long overlapEnd = endIsInside ? source.End : modificationRange.End;
                Range overlap = new(overlapStart + incrementForOverlap, overlapEnd + incrementForOverlap);
                overlappedRanges.Add(overlap);
            }
            List<Range> result = new();
            result.AddRange(overlappedRanges);

            List<Range> mapRanges = new List<Range>();
            foreach(Map m in maps)
            {
                mapRanges.Add(m.SourceRange);
            }
            nonOverlappedRanges.AddRange(GetRemainingRanges(source.Start, source.End, mapRanges));            
            result.AddRange(nonOverlappedRanges);
            return result;
        }

        static List<Range> GetRemainingRanges(long start, long end, List<Range> subtractedRanges)
        {
            List<Range> remainingRanges = new List<Range>();

            // Add the original range to remaining ranges
            remainingRanges.Add(new Range(start, end));

            // Subtract each range from the remaining ranges
            foreach (var subtractedRange in subtractedRanges)
            {
                List<Range> newRemainingRanges = new List<Range>();
                foreach (var remainingRange in remainingRanges)
                {
                    // Subtract the ranges and add the result to the new remaining ranges
                    newRemainingRanges.AddRange(remainingRange.Subtract(subtractedRange));
                }

                // Update remaining ranges with the new result
                remainingRanges = newRemainingRanges;
            }

            return remainingRanges;
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

        public Range SourceRange => new(Source, Source + Range -1);
        public long Increment => Destination - Source;

        public Map(string inputLine)
        {
            var list = RegexService.GetLongNumbers(inputLine);
            Destination = list[0];
            Source = list[1];
            Range = list[2];
        }
    }

    public class Range
    {
        public long Start { get; }
        public long End { get; }

        public long Diff => End - Start;


        public Range(long start, long end)
        {
            Start = start;
            End = end;
        }

        public List<Range> Subtract(Range subtractedRange)
        {
            List<Range> result = new List<Range>();

            // If the ranges don't overlap, just add the current range to the result
            if (End < subtractedRange.Start || Start > subtractedRange.End)
            {
                result.Add(new Range(Start, End));
            }
            else
            {
                // Handle the case where the ranges overlap
                if (Start < subtractedRange.Start)
                {
                    result.Add(new Range(Start, subtractedRange.Start - 1));
                }

                if (End > subtractedRange.End)
                {
                    result.Add(new Range(subtractedRange.End + 1, End));
                }
            }

            return result;
        }
    }
}
