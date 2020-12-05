using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day5 {
	class Range {
		public char upperChar { get; set; }
		public char lowerChar { get; set; }

		private (int min, int max) currentRange = (0,0);
		private int selectedNum = 0;

		public Range(int min, int max, char upperChar, char lowerChar) {
			this.currentRange = (min, max + 1);
			this.upperChar = upperChar;
			this.lowerChar = lowerChar;
		}

		public int ProcessOperation(char opCode) {
			if (opCode == lowerChar) {
				this.currentRange.max -= ((this.currentRange.max - this.currentRange.min) / 2);

				this.selectedNum = this.currentRange.max - 1;
			}

			if (opCode == upperChar) {
				this.currentRange.min += ((this.currentRange.max - this.currentRange.min) / 2);

				this.selectedNum = this.currentRange.min;
			}

			return this.selectedNum;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllText("lines.txt").Split('\n');
        	int maxSeatId = 0;
        	var takenSeats = new List<int>();

        	foreach (var l in lines) {
        		var movements = l.ToCharArray();
        		var yRange = new Range(0, 127, 'B', 'F');
        		var xRange = new Range(0, 7, 'R', 'L');
        		(int x, int y) position = (0,0);

        		foreach (var c in movements) {
        			position.y = yRange.ProcessOperation(c);
        			position.x = xRange.ProcessOperation(c);
        		}

        		int seatId = position.y * 8 + position.x;

        		takenSeats.Add(seatId);

        		if (seatId > maxSeatId) {
        			maxSeatId = seatId;
        		}
        	}

        	Console.WriteLine($"Max seat id {maxSeatId}");

        	int mySeat = 0;
        	for (var i = 0; i < maxSeatId; i++) {
        		if (!takenSeats.Any(x => x == i)) {
        			mySeat = i;
        		}
        	}

        	Console.WriteLine($"My seat is {mySeat}");
        }
    }
}
