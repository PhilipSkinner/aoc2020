using System;
using System.Linq;
using System.Collections.Generic;

namespace day23 {
	class Cup : IComparable<Cup> {
		public int Label { get; set; }
		public Cup NextCup { get; set; }

		public Cup(int Label) {
			this.Label = Label;
		}

		public int CompareTo(Cup other) {
			return Label.CompareTo(other.Label);
		}
	}

    class Program {
        static void Main(string[] args) {
        	var cups = "219748365".ToCharArray().Select(x => new Cup(int.Parse($"{x}"))).ToList();
        	var lookup = new Dictionary<int, Cup>();

        	for (var i = cups.Max().Label + 1; i <= 1000000; i++) {
        		cups.Add(new Cup(i));
        	}

        	for (var i = 0; i < cups.Count; i++) {
        		lookup[cups[i].Label] = cups[i];
        	}

        	//we have a loop of cups!
        	for (var i = 0; i < cups.Count; i++) {
        		if (i == cups.Count - 1) {
        			cups[i].NextCup = cups[0];
    			} else {
    				cups[i].NextCup = cups[i+1];
    			}
        	}

        	var currentCup = cups[0];
        	var initialCup = cups[0];
        	var turn = 0;
        	while (turn < 10000000) {
				//get the next three cups
        		var cupSelection = new List<Cup>() { currentCup.NextCup };
        		cupSelection.Add(cupSelection.Last().NextCup);
        		cupSelection.Add(cupSelection.Last().NextCup);

        		//skip these cups
    			currentCup.NextCup = cupSelection.Last().NextCup;

    			var targetLabel = currentCup.Label - 1;
    			while (cupSelection.Any(c => c.Label == targetLabel)) {
    				targetLabel--;
    			}

    			if (targetLabel == 0) {
    				targetLabel = cups.Where(x => !cupSelection.Any(c => c.Label == x.Label)).Max().Label;
    			}

        		var targetCup = lookup[targetLabel];

        		cupSelection.Last().NextCup = targetCup.NextCup;
        		targetCup.NextCup = cupSelection.First();

        		currentCup = currentCup.NextCup;

        		turn++;
        	}

        	var finalCups = new List<Cup>() { lookup[1].NextCup, lookup[1].NextCup.NextCup };

        	Console.WriteLine($"Cups are {finalCups[0].Label} and {finalCups[1].Label} which equals {(long)finalCups[0].Label * (long)finalCups[1].Label}");
        }
    }
}
