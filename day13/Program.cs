using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace day13 {
	class Bus {
		public long Id { get; set; }
		public long Ord { get; set; }
		public long CurrentTime { get; set; }
		public Bus NextBus { get; set; }

		public bool HasAligned(long time) {
			return this.HasArrived(time) && (this.NextBus == null || this.NextBus.HasAligned(time));
		}

		public bool HasArrived(long time) {
			return (time + this.Ord) % this.Id == 0 && time != 0;
		}
	}

    class Program {
    	static int startTime = 939;
    	static List<Bus> buses = new List<Bus>();

    	static void ParseBuses(string line) {
    		var parts = line.Split(",");

    		int ordinal = 0;
    		foreach (var p in parts) {
    			int id;
    			if (int.TryParse(p, out id)) {
    				buses.Add(new Bus() {
    					Id = id,
    					Ord = ordinal,
    					CurrentTime = 0
					});
    			}
    			ordinal++;
    		}

    		//link each of the buses
    		for (var i = 0; i < buses.Count -1; i++) {
    			buses[i].NextBus = buses[i+1];
    		}
    	}

        static void Main(string[] args) {
        	//read the data and force its processing
        	var lines = File.ReadAllLines("data.txt");
        	lines.Take(1).ToList().ForEach(x => startTime = int.Parse(x));
            lines.Skip(1).ToList().ForEach(x => ParseBuses(x));

            long start = 0;
            long increment = buses[0].Id;
            int busNum = 0;

            while (!buses[0].HasAligned(start)) {
            	start += increment;

            	if (busNum < buses.Count - 1 && buses[busNum].HasArrived(start) && buses[busNum+1].HasArrived(start)) {
					//move onto the next bus
            		busNum++;

            		if (busNum >= buses.Count) {
            			busNum = buses.Count - 1;
            		}

        			//we need to work out our next increment value
            		increment *= buses[busNum].Id;
            		Console.WriteLine($"{start} Increment is now {increment}");
        		}
            }

            Console.WriteLine($"Time is {start}");
        }
    }
}