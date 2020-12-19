using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PCRE;

namespace day19 {
	class Ruleset {
		public Dictionary<int, string> AbsoluteValues { get; set; }
		public Dictionary<int, string> RuleValues { get; set; }
		public List<string> ValidValues { get; set; }
		private Regex HasNumber = new Regex(@"\d");
		private PcreRegex Groupings = new PcreRegex(@"(\((?>[^()]+|(?1))*\))");
		private Hashtable Cache = new Hashtable();
		private Regex _IsValid;

		public Ruleset(List<string> input) {
			this.AbsoluteValues = new Dictionary<int, string>();
			this.RuleValues = new Dictionary<int, string>();

			for (var i = 0; i < input.Count; i++) {
				var rule = input[i];
				if (rule.IndexOf("\"") != -1) {
					this.AbsoluteValues[i] = rule.Replace("\"", "").Trim();
				} else {
					this.RuleValues[i] = rule.Trim();
				}
			}

			int count = 0;
			while (this.RuleValues.Keys.Count > 0) {
				var newValues = this.RuleValues.ToDictionary(entry => entry.Key, entry => entry.Value);
				foreach (var k in newValues.Keys) {
					var nums = newValues[k].Split("|").Select(x => {
						return x.Trim().Split(" ").ToList();
					}).ToList();

					nums.ForEach(n => {
						n.ForEach(j => {
							if (int.TryParse(j, out int ignore) && this.AbsoluteValues.ContainsKey(int.Parse(j))) {
								var v = this.AbsoluteValues[int.Parse(j)];

								if (v.Length != 1) {
									v = $"({v})";
								}

								this.RuleValues[k] = $" {this.RuleValues[k]}".Replace($" {j}", $" {v}").Trim();
							}
						});
					});
				}

				foreach (var r in this.RuleValues.Keys) {
					if (!this.HasNumber.IsMatch(this.RuleValues[r])) {
						//add it to our absolute values
						this.AbsoluteValues[r] = this.RuleValues[r];
						newValues.Remove(r);
					} else {
						newValues[r] = this.RuleValues[r];
					}
				}

				count++;

				if (count > 10) {
					//nice
					foreach (var r in this.RuleValues.Keys) {
						if (r == 8) {
							this.AbsoluteValues[r] = $"({this.RuleValues[r].Replace(" 8", "")})+";
							newValues.Remove(r);
						}

						if (r == 11) {
							//just guestimated from the file
							var woot = Enumerable.Range(1, 46).Select(x => {
								return $"({this.AbsoluteValues[42]}){{{x}}}({this.AbsoluteValues[31]}){{{x}}}";
							}).ToList();
							this.AbsoluteValues[r] = $"({string.Join("|", woot)})";
							newValues.Remove(r);
						}
					}
				}

				if (count > 11) {
					this.AbsoluteValues[0] = this.RuleValues[0];
					newValues.Remove(0);
				}

				this.RuleValues = newValues;
			}

			this._IsValid = new Regex($"^{this.AbsoluteValues[0].Replace(" ", "")}$");
		}

		public bool IsValid(string message) {
			return this._IsValid.Match(message).Success;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllLines("rules.txt").ToList();
        	var lookup = new SortedDictionary<int, string>();

        	lines.ForEach(l => {
        		var p = l.Split(":");
        		lookup[int.Parse(p[0])] = p[1];
    		});

        	var rules = new Ruleset(lookup.Keys.ToList().Select(x => lookup[x].Trim()).ToList());

        	var messages = File.ReadAllLines("messages.txt").ToList();
        	var result = messages.Sum(x => rules.IsValid(x) ? 1 : 0);

        	Console.WriteLine($"Result is {result}");
        }
    }
}
