using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day15 {
    class Program {
        static void Main(string[] args) {
            var numbers = File.ReadAllText("data.txt").Split(",").Select(x => int.Parse(x)).ToList();
            var record = new Dictionary<int, int>();
            var history = new Dictionary<int, List<int>>();
            var lastSpoken = -1;

            //speak all the first numbers
            for (var i = 0; i < numbers.Count; i++) {
            	lastSpoken = numbers[i];
            	record[lastSpoken] = 1;
            	history[lastSpoken] = new List<int>() {
            		i
            	};
            }

            for (var i = numbers.Count; i < 30000000; i++) {
            	if (record[lastSpoken] == 1) {
            		//our new number
            		lastSpoken = 0;
            	} else {
            		//get the last two elements
            		var length = history[lastSpoken].Count - 1;
            		lastSpoken = history[lastSpoken][length] - history[lastSpoken][length - 1];
            	}

            	if (i % 1000000 == 0) {
            		Console.WriteLine($"Speaking {i}");
            	}


            	if (!record.ContainsKey(lastSpoken)) {
            		record[lastSpoken] = 0;
            		history[lastSpoken] = new List<int>();
            	}

            	record[lastSpoken]++;
            	history[lastSpoken].Add(i);
            }

            Console.WriteLine($"{lastSpoken}");
        }
    }
}
