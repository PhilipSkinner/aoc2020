using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16 {
    class Rule {
        public string Name { get; set; }
        public List<(int start, int end)> Ranges { get; set; }
        public bool Known { get; set; }

        public Rule(string line) {
            var parts = line.Split(":");
            this.Name = parts[0];

            this.Ranges = parts[1].Split(" or ").Select(x => {
                var minMax = x.Split("-").Select(y => int.Parse(y)).ToList();
                return (minMax[0], minMax[1]);
            }).ToList();

            this.Known = false;
        }

        public bool IsValid(int num) {
            return !this.Known && this.Ranges.Any(x => x.start <= num && x.end >= num);
        }
    }

    class Program {
        static bool AllMatch(Rule r, List<int> values) {
            return values.All(x => r.IsValid(x));
        }

        static List<List<int>> Convert(List<List<int>> input)
        {
            var result = new List<List<int>>();
            for (int i = 0; i < input[0].Count(); i++) {
                result.Add(input.Select(x => x[i]).ToList());
            }
            return result;
        }

        static void Main(string[] args) {
            var myTicket = new List<long>() { 61,151,59,101,173,71,103,167,127,157,137,73,181,97,179,149,131,139,67,53 };
            var otherTickets = File.ReadAllLines("tickets.txt").Select(x => x.Split(",").Select(y => int.Parse(y)).ToList()).ToList();
            var validTickets = new List<List<int>>();
            var rules = File.ReadAllLines("rules.txt").Select(x => new Rule(x)).ToList();

            int errorRate = 0;
            otherTickets.ForEach(t => {
                var tValid = true;
                int index = 0;

                t.ForEach(n => {
                    var res = rules.Any(x => x.IsValid(n));
                    errorRate += res ? 0 : n;
                    tValid = tValid && res;

                    index++;
                });

                if (tValid) {
                    validTickets.Add(t);
                }
            });

            var rec = new List<string>();
            var result = new List<int>();
            var values = Convert(validTickets);

            var matchingFields = values.Select((x, i) => (
                Fields : rules.Where(y => AllMatch(y, x)).Select(y => y.Name),
                Index : i
            )).OrderBy(x => x.Fields.Count()).ToList();

            for (var i = 0; i < values.Count(); i++) {
                var firstMatch = matchingFields.First().Fields.First();

                if (firstMatch.Contains("depart")) {
                    result.Add(matchingFields.First().Index);
                }

                rec.Add(firstMatch);
                matchingFields = matchingFields.Select(x => (
                    Fields: x.Fields.Where(y => !rec.Contains(y)),
                    Index : x.Index
                )).Where(x => x.Fields.Count() > 0)
                .OrderBy(x => x.Fields.Count()).ToList();
            }

            var nums = result.Select(x => myTicket[x]).ToList();
            long num = nums.Aggregate((x, y) => x * y);

            Console.WriteLine($"{num}");
            Console.WriteLine($"Error rate : {errorRate}");
            Console.WriteLine($"Number valid tickets : {validTickets.Count}");

        }
    }
}
