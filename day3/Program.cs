using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day3 {
	class Coordinate {
		public int x { get; set; }
		public int y { get; set; }
		private int xMax { get; set; }
		private int yMax { get; set; }

		public Coordinate(int xMax, int yMax) {
			this.xMax = xMax;
			this.yMax = yMax;
		}

		public bool MoveBy(int x, int y) {
			if (this.y == this.yMax && y > 0) {
				return false;
			}

			this.x += x;
			this.y += y;

			if (this.x > this.xMax) {
				this.x -= this.xMax + 1;
			}

			return true;
		}
	}

	class Row {
		public List<bool> cells { get; set;}

		public Row(string map) {
			this.cells = map.ToCharArray().Select(x => x != '.').ToList();
		}
	}

	class Map {
		public List<Row> rows { get; set; }
		private Coordinate location { get; set; }
		public int collisions { get; set; }

		public Map(string[] lines) {
			this.collisions = 0;
			this.rows = new List<Row>();
			foreach (var l in lines) {
				this.rows.Add(new Row(l));

			}

			this.location = new Coordinate(this.rows.First().cells.Count - 1, this.rows.Count - 1);
		}

		public bool MoveBy(int x, int y) {
			if (this.location.MoveBy(x, y)) {
				//move was valid

				//did we land on a tree?
				if (this.rows[this.location.y].cells[this.location.x]) {
					this.collisions++;
				}

				return true;
			}

			return false;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllText("map.txt").Split('\n');

        	var combinations = new List<(int, int)>() {
				(1, 1),
				(3, 1),
				(5, 1),
				(7, 1),
				(1, 2)
        	};
        	var num = 1;

        	foreach (var c in combinations) {
        		var map = new Map(lines);

        		while (map.MoveBy(c.Item1, c.Item2)) {

        		}

        		Console.WriteLine($"Slope {c.Item1} right and {c.Item2} down total collisions: {map.collisions}");

        		num *= map.collisions;
        	}

        	Console.WriteLine($"Answer is {num}");
        }
    }
}
