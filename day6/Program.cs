using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day6 {
    class Program {
    	//nasty
        static void Main(string[] args) {
        	var lines = File.ReadAllText("lines.txt").Split("\n\n");
            var summed = lines.Select(x => x.Replace("\n", "").Distinct().Count()).Sum();
            int tot = 0;

            foreach (var l in lines) {
            	var chars = l.Replace("\n", "").Distinct();
            	var allAnswered = string.Join("", l.Replace("\n", "").Distinct());

            	foreach (var c in chars) {
	            	foreach (var g in l.Split("\n")) {
	            		if (g.IndexOf(c) == -1 && !string.IsNullOrWhiteSpace(g)) {
	            			allAnswered = allAnswered.Replace($"{c}", "");
	            		}
	            	}
            	}

            	Console.WriteLine($"Complete is {l}, answered is {allAnswered}");
            	tot += allAnswered.Length;
            }


            Console.WriteLine($"Total is {summed}");
            Console.WriteLine($"Second is {tot}");
        }
    }
}
