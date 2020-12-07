using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day7 {
	class BagRelation {
		public int count { get; set; }
		public Bag bagType { get; set; }
	}

	class Bag {
		public string name { get; set; }
		private readonly List<string> rules;
		public List<BagRelation> canContain { get; set; }
		public List<Bag> canBeContainedBy { get; set; }
		private readonly Regex ruleRegex = new Regex(@"^(\d+) (.*?) bags?");

		public Bag(string colour, string rules) {
			this.name = colour;
			this.rules = rules.Split(", ").ToList();
			this.canContain = new List<BagRelation>();
			this.canBeContainedBy = new List<Bag>();
		}

		public void ProcessRules(List<Bag> allBags) {
			foreach (var r in rules) {

				var matches = this.ruleRegex.Match(r);

				if (matches.Success) {
					var foundBagType = allBags.Single(x => x.name == matches.Groups[2].Value);

					if (!this.canContain.Any(x => x.bagType.name == matches.Groups[2].Value)) {
						this.canContain.Add(new BagRelation() {
							count = int.Parse(matches.Groups[1].Value),
							bagType = foundBagType
						});
					}

					if (!foundBagType.canBeContainedBy.Any(x => x.name == this.name)) {
						foundBagType.canBeContainedBy.Add(this);
					}
				}
			}
		}

		public bool CanContain(string bag) {
			foreach (var b in this.canContain) {
				if (b.bagType.name == bag) {
					return true;
				}

				if (b.bagType.CanContain(bag)) {
					return true;
				}
			}

			return false;
		}

		public int Contains() {
			int ret = this.canContain.Sum(x => x.count);

			foreach (var b in this.canContain) {
				ret += b.bagType.Contains() * b.count;
			}

			return ret;
		}
	}

    class Program {
        static void Main(string[] args) {
            var lines = File.ReadAllLines("lines.txt");
            var bags = new List<Bag>();

            foreach (var l in lines) {
            	var parts = l.Split(" bags contain ");

            	if (!bags.Any(x => x.name == parts[0])) {
            		bags.Add(new Bag(parts[0], parts[1]));
            	}
            }

            //process each bag to set up its relationships
            foreach (var b in bags) {
            	b.ProcessRules(bags);
            }

            //now find the gold bag options
            int total = bags.Count(x => x.CanContain("shiny gold"));

            Console.WriteLine($"Total is {total}");

            int shinyGoldCount = bags.Single(x => x.name == "shiny gold").Contains();

            Console.WriteLine($"Bag count is {shinyGoldCount}");
        }
    }
}
