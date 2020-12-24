using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day24 {
	class Instruction {
		public List<string> Directions { get; set; }
		private readonly Regex parser = new Regex(@"(e|se|sw|w|nw|ne)");

		public Instruction(string input) {
			this.Directions = new List<string>();

			var matches = parser.Matches(input);

			for (var i = 0; i < matches.Count; i++) {
				this.Directions.Add(matches[i].Value);
			}
		}

		public (int X, int Y) ApplyDirections((int X, int Y) start) {
			foreach (var d in this.Directions) {

				switch (d) {
					case "w":
						start.X -= 1;
						break;
					case "e":
						start.X += 1;
						break;
					case "sw":
						start.X -= 1;
						start.Y -= 1;
						break;
					case "se":
						start.Y -= 1;
						break;
					case "nw":
						start.Y += 1;
						break;
					case "ne":
						start.X += 1;
						start.Y += 1;
						break;
				}
			}

			return start;
		}
	}

	class Tile {
		public (int X, int Y) Coords { get; set; }
		public bool IsBlack { get; set; }
		public bool ShouldFlip { get; set; }

		public void PopulateAdjacent(Dictionary<(int X, int Y), Tile> allTiles) {
			var toCheck = new List<(int X, int Y)>() {
				(this.Coords.X - 1, this.Coords.Y), 	// w
				(this.Coords.X + 1, this.Coords.Y), 	// e
				(this.Coords.X - 1, this.Coords.Y - 1),	// sw
				(this.Coords.X, this.Coords.Y - 1), 	// se
				(this.Coords.X, this.Coords.Y + 1), 	// nw
				(this.Coords.X + 1, this.Coords.Y + 1)	// ne
			};

			foreach (var t in toCheck) {
				if (!allTiles.ContainsKey(t)) {
					//need to set it to a white tile
					allTiles[t] = new Tile() {
						IsBlack = false,
						Coords 	= t
					};
				}
			}
		}

		public void DetermineState(Dictionary<(int X, int Y), Tile> allTiles) {
			this.ShouldFlip = false;

			var adjacent = new List<Tile>();

			var toCheck = new List<(int X, int Y)>() {
				(this.Coords.X - 1, this.Coords.Y), 	// w
				(this.Coords.X + 1, this.Coords.Y), 	// e
				(this.Coords.X - 1, this.Coords.Y - 1),	// sw
				(this.Coords.X, this.Coords.Y - 1), 	// se
				(this.Coords.X, this.Coords.Y + 1), 	// nw
				(this.Coords.X + 1, this.Coords.Y + 1)	// ne
			};

			foreach (var t in toCheck) {
				if (!allTiles.ContainsKey(t)) {
					//need to set it to a white tile
					allTiles[t] = new Tile() {
						IsBlack = false,
						Coords 	= t
					};
				}

				adjacent.Add(allTiles[t]);
			}

			var blackAdjacent = adjacent.Count(x => x.IsBlack);

			if (this.IsBlack && (blackAdjacent == 0 || blackAdjacent > 2)) {
				this.ShouldFlip = true;
			}

			if (!this.IsBlack && blackAdjacent == 2) {
				this.ShouldFlip = true;
			}
		}

		public void Action() {
			if (this.ShouldFlip) {
				this.IsBlack = !this.IsBlack;
			}

			this.ShouldFlip = false;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllLines("data.txt").Select(x => new Instruction(x)).ToList();
        	var tiles = new Dictionary<(int X, int Y), Tile>();

        	lines.ForEach(l => {
        		var currentLocation = l.ApplyDirections((0, 0));

        		if (!tiles.ContainsKey(currentLocation)) {
        			tiles[currentLocation] = new Tile() {
        				IsBlack = false,
        				Coords 	= currentLocation
        			};

        			tiles[currentLocation].PopulateAdjacent(tiles);
        		}

        		tiles[currentLocation].IsBlack = !tiles[currentLocation].IsBlack;
    		});

    		var allTiles = tiles.Values.ToList();
    		allTiles.ForEach(t => t.PopulateAdjacent(tiles));

    		for (var i = 0; i < 100; i++) {
    			allTiles = tiles.Values.ToList();
    			allTiles.ForEach(t => t.DetermineState(tiles));
    			allTiles.ForEach(t => t.Action());

				Console.WriteLine($"Day {i + 1}: {allTiles.Where(x => x.IsBlack).Count()}");
    		}
        }
    }
}
