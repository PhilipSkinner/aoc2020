using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace day10 {
    class Program {
    	static Stopwatch timer = new Stopwatch();
    	static Hashtable cache = new Hashtable();

    	static long GetValidPaths(List<int> lines, int currentJoltage) {
    		long ret = 0;

    		for (var i = 0; i < lines.Count; i++) {
    			var l = lines[i];

    			if (currentJoltage - l > 3) {
    				break;
    			}

    			if (l >= 1 && l <= 3) {
    				ret++;
    			}

    			var nextLines = lines.Skip(i + 1).ToList();
    			var cacheKey = $"{string.Join(",", nextLines)}-{currentJoltage}";
    			if (cache.Contains(cacheKey)) {
    				ret += (long)cache[cacheKey];
				} else {
					long foundPaths = GetValidPaths(nextLines, l);
					cache.Add(cacheKey, foundPaths);
	    			ret += foundPaths;
				}

    		}

    		return ret;
    	}

        static void Main(string[] args) {
        	timer.Start();

            var lines = File.ReadAllLines("lines.txt").Select(x => int.Parse(x)).ToList();
            lines.Sort();
            lines.Reverse();

            int currentJoltage = lines.First() + 3;
            long count = GetValidPaths(lines.ToList(), currentJoltage);
            timer.Stop();

            Console.WriteLine($"{count} in {timer.ElapsedMilliseconds}ms");
        }
    }
}