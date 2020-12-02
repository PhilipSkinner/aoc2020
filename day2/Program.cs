using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day2 {
	class ParsedLine {
		private Regex r = new Regex(@"^(\d+)\-(\d+) (\w): (.*?)$");

		public int min {get; set;}
		public int max {get; set;}
		public char toFind {get; set;}
		public string password {get; set;}

		public ParsedLine(string line) {
			var match = this.r.Match(line);

			if (match.Success) {
				this.min 		= int.Parse(match.Groups[1].Value);
				this.max 		= int.Parse(match.Groups[2].Value);
				this.toFind 	= match.Groups[3].Value.ToCharArray()[0];
				this.password 	= match.Groups[4].Value;
    		}
		}

		public bool IsValidFirst() {
			if (this.password == null) {
				return false;
			}

			int num = this.password.ToCharArray().Where(x => x == this.toFind).Count();

    		return num >= this.min && num <= this.max;
		}

		public bool IsValidSecond() {
			if (this.password == null) {
				return false;
			}

			var min = this.min - 1;
			var max = this.max - 1;

    		return this.password.ToCharArray().Where((x, i) => (i == min || i == max) && x == this.toFind).Count() == 1;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllText("list.txt").Split('\n').Select(x => new ParsedLine(x));

        	Console.WriteLine($"Number valid (first phase): {lines.Where(x => x.IsValidFirst()).Count()}");
        	Console.WriteLine($"Number valid (second phase): {lines.Where(x => x.IsValidSecond()).Count()}");
        }
    }
}
