using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day17 {
    //this is super inefficient, why didn't I use arrays :(
    class CoordinateCollection {
        public List<Coordinate> Coordinates { get; set; }
        public Dictionary<(int X, int Y, int Z, int W), Coordinate> Lookup { get; set; }

        public CoordinateCollection() {
            this.Coordinates = new List<Coordinate>();
            this.Lookup = new Dictionary<(int X, int Y, int Z, int W), Coordinate>();
        }

        public void Add(Coordinate coord) {
            this.Coordinates.Add(coord);
            this.Lookup.Add((coord.X, coord.Y, coord.Z, coord.W), coord);
        }

        public Coordinate LookupCoordinate((int X, int Y, int Z, int W) loc) {
            if (this.Lookup.ContainsKey(loc)) {
                return this.Lookup[loc];
            }

            return null;
        }
    }

    class Coordinate {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
        public Cube Value { get; set; }
        public List<Cube> Adjacent { get; set; }

        public Coordinate(int X, int Y, int Z, int W, Cube Value) {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
            this.Value = Value;
        }

        public bool IsNeighbour(int x, int y, int z, int w) {
            return Math.Abs(this.X - x) <= 1
                && Math.Abs(this.Y - y) <= 1
                && Math.Abs(this.Z - z) <= 1
                && Math.Abs(this.W - w) <= 1
                && (
                    this.X != x
                    || this.Y != y
                    || this.Z != z
                    || this.W != w
                );
        }

        public List<Cube> GetAdjacent(CoordinateCollection AllCubes) {
            if (this.Adjacent != null && this.Adjacent.Count > 0) {
                return this.Adjacent;
            }

            var adjacent = new List<Cube>();

            for (var x = this.X - 1; x <= this.X + 1; x++) {
                for (var y = this.Y - 1; y <= this.Y + 1; y++) {
                    for (var z = this.Z - 1; z <= this.Z + 1; z++) {
                        for (var w = this.W - 1; w <= this.W + 1; w++) {
                            if (this.IsNeighbour(x, y, z, w)) {
                                var cube = AllCubes.LookupCoordinate((x, y, z, w));

                                if (cube == null) {
                                    cube = new Coordinate(x, y, z, w, new Cube('.'));
                                    AllCubes.Add(cube);
                                    adjacent.Add(cube.Value);
                                } else {
                                    adjacent.Add(cube.Value);
                                }
                            }
                        }
                    }
                }
            }

            this.Adjacent = adjacent;

            return this.Adjacent;
        }
    }

	class Cube {
		public bool IsActive { get; set; }
		public bool NewState { get; set; }

		public Cube(char val) {
			this.IsActive = false;

			if (val == '#') {
				this.IsActive = true;
			}

            this.NewState = this.IsActive;
		}

		public string GetState() {
			return this.IsActive ? "#" : ".";
		}

		public void ProcessAdjacencyRules(List<Cube> Adjacent) {
			var numAdjacent = Adjacent.Count(x => x.IsActive);

            this.NewState = this.IsActive;

			if (this.IsActive && (numAdjacent == 2 || numAdjacent == 3)) {
				this.NewState = true;
			} else if (!this.IsActive && numAdjacent == 3) {
				this.NewState = true;
			} else {
                this.NewState = false;
            }
		}

		public void ActionRules() {
			this.IsActive = this.NewState;
		}
	}

    class Program {
		static CoordinateCollection state = new CoordinateCollection();

        static void Main(string[] args) {
            var lines = File.ReadAllLines("state.txt").Select(x => x.ToCharArray().ToList()).ToList();

            for (var x = 0; x < lines.Count; x++) {
                for (var y = 0; y < lines[x].Count; y++) {
                    state.Add(new Coordinate(x, y, 0, 0, new Cube(lines[x][y])));
                }
            }

            Console.WriteLine($"Generating new round of cubes");
            var currentState = new CoordinateCollection() {
                Coordinates = state.Coordinates.ToList()
            };
            //update the adjacent cubes we need
            currentState.Coordinates.ForEach(x => x.GetAdjacent(state));
            Console.WriteLine($"Generated");

            int numRounds = 6;
            Console.WriteLine($"Number {state.Coordinates.Where(x => x.Value.IsActive).Count()}\n\n");
            //cycle some rounds
            for (var i = 0; i < numRounds; i++) {
                //output
                Console.WriteLine($"After {i+1} Cycles:");

                //then process
                Console.WriteLine($"Processing");
                currentState = new CoordinateCollection() {
                    Coordinates = state.Coordinates.ToList()
                };
            	currentState.Coordinates.ForEach(x => x.Value.ProcessAdjacencyRules(x.GetAdjacent(state)));
                currentState.Coordinates.ForEach(x => x.Value.ActionRules());
                Console.WriteLine($"Completed");

                Console.WriteLine($"Number {state.Coordinates.Where(x => x.Value.IsActive).Count()}\n\n");
            }
        }
    }
}
