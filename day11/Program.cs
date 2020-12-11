using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day11 {
	//wanted to create a "simulation" with entities backing the objects in the problem - not the quickest or most concise!
	class Seat {
		public bool IsSeat { get; set ;}
		public bool IsOccupied { get; set; }
		private bool ShouldToggleOccupancy { get; set; }
		public List<Seat> Adjacent { get; set; }

		public Seat(char val) {
			this.IsOccupied = false;
			this.Adjacent = new List<Seat>();

			if (val == '.') {
				this.IsSeat = false;
			}

			if (val == 'L') {
				this.IsSeat = true;
			}

			this.ShouldToggleOccupancy = false;
		}

		public string GetState() {
			return this.IsSeat ? (this.IsOccupied ? "#" : "L") : ".";
		}

		public void SetAdjacent(List<Seat> adj) {
			this.Adjacent = adj.ToList();
		}

		public void ProcessAdjacencyRules() {
			if (!this.IsSeat) {
				return;
			}

			if (!this.IsOccupied && !this.Adjacent.Any(x => x.IsOccupied)) {
				this.ShouldToggleOccupancy = true;
			} else if (this.IsOccupied && this.Adjacent.Count(x => x.IsOccupied) > 4) {
				this.ShouldToggleOccupancy = true;
			}
		}

		public void ActionRules() {
			if (this.ShouldToggleOccupancy) {
				this.IsOccupied = !this.IsOccupied;
			}

			this.ShouldToggleOccupancy = false;
		}
	}

	class Row {
		public List<Seat> Seats { get; set; }

		public Row(string line) {
			this.Seats = line.ToCharArray().Select(x => new Seat(x)).ToList();
		}

		public string OutputState() {
			var state = "";
    		foreach (var s in this.Seats) {
        		state = ($"{state}{s.GetState()}");
        	}
	    	return state;
		}
	}

    class Program {
    	static int diff((int, int) one, (int, int) two) {
    		return Math.Abs(one.Item1 - two.Item1) + Math.Abs(one.Item2 - two.Item2);
    	}

        static void Main(string[] args) {
            var rows = File.ReadAllLines("lines.txt").Select(x => new Row(x)).ToList();
            var hasChanged = true;

            //now we need to set our adjacency
            var max = rows.Count * 2;
            for (var i = 0; i < rows.Count; i++) {
            	for (var j = 0; j < rows[i].Seats.Count; j++) {
            		var adjacency = new List<(int, int)>() {
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            			(-max, -max),
            		};

            		//new adjacency rules for which seat is considered adjacent
            		for (var l = 0; l < rows.Count; l++) {
            			for (var k = 0; k < rows[i].Seats.Count; k++) {
            				//should we consider this seat for adjacency?
            				var seat = rows[l].Seats[k];
            				int applyTo = -1;

            				if (seat.IsSeat) {
            					//ok, is it vertically above/below
            					if (l != i && k == j) {
            						if (l < i) {
										applyTo = 1;
        							} else {
        								applyTo = 7;
        							}
            					}

            					//horizontally in line
            					if (l == i && k != j) {
            						if (k < j) {
            							applyTo = 3;
            						} else {
            							applyTo = 5;
            						}
            					}

            					//diagonal
            					if (l != i && k != j && Math.Abs(i - l) == Math.Abs(j - k)) {
            						if (l < i) {
        						 		if (k < j) {
        						 			//upper left
        						 			applyTo = 0;
            						 	} else {
            						 		//upper right
            						 		applyTo = 2;
            						 	}
            						}

            						if (l > i) {
            							if (k < j) {
            								//lower left
            								applyTo = 6;
        								} else {
        									//lower right
        									applyTo = 8;
        								}
            						}
            					}
            				}

            				if (applyTo > -1) {
            					adjacency[applyTo] = diff(adjacency[applyTo], (i, j)) > diff((l, k), (i, j)) ? (l, k) : adjacency[applyTo];
            				}
            			}
            		}

        			rows[i].Seats[j].SetAdjacent(adjacency.Where(x => x != (-max, -max)).Select(x => rows[x.Item1].Seats[x.Item2]).ToList());
            	}
            }

            var currentState = "";
            while (hasChanged) {
            	rows.ForEach(r => r.Seats.ForEach(s => s.ProcessAdjacencyRules()));
            	rows.ForEach(r => r.Seats.ForEach(s => s.ActionRules()));
            	var state = string.Join("\n", rows.Select(r => r.OutputState()));

            	hasChanged = currentState != state;
            	currentState = state;
            }

            Console.WriteLine($"Total occupied: {rows.Select(r => r.Seats.Select(s => s.IsOccupied ? 1 : 0).Sum()).Sum()}");
        }
    }
}
