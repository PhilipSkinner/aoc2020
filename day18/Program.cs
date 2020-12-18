using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace day18 {
    class Program {
    	static Regex parenthesisRegex = new Regex(@"(\([\d \*\+]*\))");
    	static Regex encRegex = new Regex(@"([^(\d]\d+ \+ \d+[^)\d]|^\d+ \+ \d+|\d+ \+ \d+$)");

    	static long Evaluate(long left, long right, string op) {
    		switch (op) {
    			case "+" :
    				return left + right;
    			case "*":
    				return left * right;
    		}

    		return 0;
    	}

    	static long EvaluateSequence(long res, string op, List<string> sequence) {
        	sequence.ForEach(s => {
        		long num = 0;
        		if (long.TryParse(s, out num)) {
        			res = Evaluate(res, num, op);
    			} else {
        			op = s;
    			}
    		});

    		return res;
    	}

    	static List<string> Expand(string sum) {
    		sum = Encapsulate(sum);

    		while (sum.IndexOf("(") != -1) {
    			//expand and replace
    			var matches = parenthesisRegex.Match(sum);
    			for (var i = 1; i < matches.Groups.Count; i++) {
    				var result = EvaluateSequence(0, "+", Expand(Encapsulate(matches.Groups[i].Value.Remove(matches.Groups[i].Value.Length - 1).Remove(0, 1))));
    				sum = sum.Replace(matches.Groups[i].Value, $"{result}");
    			}
    		}

    		var newSum = Encapsulate(sum);

    		if (newSum != sum) {
    			return Expand(newSum);
    		}

			return sum.Trim().Split(" ").ToList();
    	}

    	static string Encapsulate(string sum) {
    		while (encRegex.IsMatch(sum)) {
    			var matches = encRegex.Match(sum);

    			var start = matches.Groups[1].Index;
    			var length = matches.Groups[1].Length;

    			if (matches.Groups[1].Value.StartsWith(" ")) {
    				start++;
    				length--;
    			}

    			if (matches.Groups[1].Value.EndsWith(" ")) {
    				length--;
    			}

    			if (start == 0 && length == sum.Length) {
    				return sum;
    			}

				sum = $"{sum.Substring(0, start)}({sum.Substring(start, length).Trim()}){sum.Substring(start + length)}";
    		}

    		return sum;
    	}

        static void Main(string[] args) {
        	var lines = File.ReadAllLines("data.txt");

    		Console.WriteLine($"Result is {lines.Sum(x => EvaluateSequence(0, "+", Expand($" {x} ")))}");
        }
    }
}
