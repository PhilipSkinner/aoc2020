using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day9 {
    class Program {
    	static bool ContainsTupleSum(long required, IEnumerable<long> numbers) {
    		return (
    			from entry in (
    				from first in numbers
    				from second in numbers
    				select new {first, second}
    			)
    			where
    				entry.first + entry.second == required
    				&& entry.first != entry.second
    			select 1
    		).Any();
    	}

    	static long ContainsContiguousSet(long required, IEnumerable<long> numbers) {
    		long val = 0;
    		List<long> range = new List<long>();
    		foreach (var n in numbers) {
    			val += n;
    			range.Add(n);

    			if (val > required) {
    				return 0;
    			}

    			if (val == required && n != required) {
    				return range.Min() + range.Max();
    			}
    		}

    		return 0;
    	}

        static void Main(string[] args) {
			var lines = File.ReadAllLines("lines.txt").Select(x => long.Parse(x)).ToList();

        	int preambleAmount = 25;

        	long toFind = 0;
        	for (var i = preambleAmount; i < lines.Count; i++) {
        		if (!ContainsTupleSum(lines[i], lines.Skip(i - preambleAmount).Take(preambleAmount))) {
        			toFind = lines[i];
        			break;
        		}
        	}

        	for (var i = 0; i < lines.Count; i++) {
        		var result = ContainsContiguousSet(toFind, lines.Skip(i));
        		if (result  > 0) {
        			Console.WriteLine($"Result is {result}");
        			break;
        		}
        	}
        }
    }
}
