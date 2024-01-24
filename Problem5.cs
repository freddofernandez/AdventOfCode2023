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
            return 1;
            // Almanac almanac = new(input[0]);
            // input.RemoveAt(0);
            // foreach (var line in input)
            // {
            //     if (string.IsNullOrEmpty(line)) continue;
            //     if (RegexService.SingleMatchPattern(line, @":").Success)
            //     {
            //         almanac.AddConversionTable();
            //     }
            //     else
            //     {
            //         almanac.UpdateLatestConversion(line);
            //     }
            // }
            //
            // var result = almanac.GetLowestLocationForSeeds();
            //
            // return result;
        }

        public static long SolveB(List<string> input)
        {
            // get all seeds
            Almanac almanac = new(input[0]);
            input.RemoveAt(0);
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (RegexService.SingleMatchPattern(line, @":").Success)
                {
                    almanac.AddTable();
                }
                else
                {
                    almanac.UpdateLastTable(line);
                }
            }

            var seedsToIterate = new List<Range>();
            seedsToIterate.AddRange(almanac.Seeds);
            foreach (var table in almanac.ConversionTables)
            {
                var nextSeeds = new List<Range>();
                foreach (var seed in seedsToIterate)
                {
                    foreach (var mapping in table)
                    {
                        var intersection = seed.GetIntersection(mapping.MappingRange);
                        if (intersection is not null)
                        {
                            intersection.ApplyModifier(mapping.Modifier);
                            nextSeeds.Add(intersection);
                            nextSeeds.AddRange(seed.GetLeftoversFromIntersection(mapping.MappingRange));
                        }
                        else
                        {
                            nextSeeds.Add(seed);
                        }
                        
                    }
                }
                seedsToIterate.Clear();
                seedsToIterate.AddRange(nextSeeds);
            }

            var result = seedsToIterate.OrderBy(x => x.Start).First().Start;
            Console.WriteLine("Lowest result found:" + result);
            return result;
            // foreach list of mappings
            // cycle each seed to check for intersections
            // if intersection is found -> save it in seeds for next cycle
            // after going through all intersections, get substractions and add them to next cycle as they are
        }

        private class Almanac
        {
            public List<Range> Seeds { get; set; } = [];
            public List<List<Mapping>> ConversionTables { get; set; } = [];

            public Almanac(string seeds)
            {
                var input = RegexService.GetLongNumbers(seeds);
                for (var i = 0; i < input.Count; i += 2)
                {
                    Seeds.Add(new Range(input[i], input[i] + input[i+1] - 1));
                }
            }

            public void AddTable()
            {
                ConversionTables.Add(new());
            }

            public void UpdateLastTable(string input)
            {
                var list = RegexService.GetLongNumbers(input);
                var destination = list[0];
                var source = list[1];
                var size = list[2];
                
                ConversionTables.Last().Add(new(source, size, destination-source));
            }
        }

        private class Mapping(long start, long size, long modifier)
        {
            public Range MappingRange { get; set; } = new(start, start + size - 1);
            public long Modifier { get; set; } = modifier;
        }

        private class Range(long start, long end)
        {
            public long Start { get; set; } = start;
            public long End { get; set; } = end;

            public Range? GetIntersection(Range intersectingRange)
            {
                if (this.Start > intersectingRange.End || this.End < intersectingRange.Start)
                {
                    return null;
                }

                if (this.Start > intersectingRange.Start)
                {
                    var ending = this.End < intersectingRange.End ? this.End : intersectingRange.End;
                    return new Range(this.Start, ending);
                }

                return new Range(intersectingRange.Start, this.End);
            }

            public List<Range> GetLeftoversFromIntersection(Range substractor)
            {
                List<Range> result = [];
                if (this.Start > substractor.End || this.End < substractor.Start)
                {
                    result.Add(this);
                    return result;
                }

                if (substractor.Start > this.Start)
                {
                    result.Add(new Range(this.Start, substractor.Start - 1));
                }

                if (substractor.End < this.End)
                {
                    result.Add(new Range(substractor.End + 1, this.End));
                }

                return result;
            }

            public void ApplyModifier(long modifier)
            {
                this.Start += modifier;
                this.End += modifier;
            }
        }
    }
}