using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day14 {
    class Program {
    	static List<string> InitMemory(string def) {
    		return Enumerable.Range(1, 36).Select(x => def).ToList();
    	}

    	static List<string> GetBitmask(string line) {
    		if (line.IndexOf("mask = ") == 0) {
    			return line.Split(" ")[2].ToCharArray().ToList().Select(x => $"{x}").ToList();
    		}

    		return null;
    	}

    	static (long location, List<string> bitmask) GetCommand(string line) {
    		if (line.IndexOf("mem[") == 0) {
    			var parts = line.Split(" ");
    			var bitmask = Convert
    						.ToString(long.Parse(parts[2]), 2)
    						.PadLeft(36, '0');
    			bitmask = bitmask.Substring(bitmask.Length - 36, 36);

    			return (
    				long.Parse(parts[0].Replace("mem[", "").Replace("]", "")),
					bitmask
						.ToCharArray()
						.Select(x => $"{x}")
						.ToList()
				);
    		}

    		return (-1, null);
    	}

    	static List<string> ApplyBitmask(List<string> source, List<string> toApply) {
    		Console.WriteLine($"Source   : {string.Join("", source)}");
    		Console.WriteLine($"Applying : {string.Join("", toApply)}");

    		var ret = InitMemory("0");
    		for (var i = 0; i < toApply.Count; i++) {
    			ret[i] = toApply[i] != "0" ? toApply[i] : source[i];
    		}

    		Console.WriteLine($"Result   : {string.Join("", ret)}");
    		return ret;
    	}

    	static List<List<string>> GeneratePermutations(List<string> source) {
    		var ret = new List<List<string>>();

    		for (var i = 0; i < source.Count; i++) {
    			if (i == source.Count - 1) {
    				if (source[i] == "X") {
    					var target = source.ToList();
	    				target[i] = "1";
	    				ret.Add(target);
	    				target = source.ToList();
	    				target[i] = "0";
    					ret.Add(target);
    				} else {
    					ret.Add(source);
    				}

    				break;
    			}

    			if (source[i] == "X") {
    				var target = source.ToList();
    				target[i] = "1";
    				ret = ret.Concat(GeneratePermutations(target)).ToList();
    				target = source.ToList();
    				target[i] = "0";
    				ret = ret.Concat(GeneratePermutations(target)).ToList();

    				break;
    			}
    		}

    		return ret;
    	}

        static void Main(string[] args) {
            var memory = new Dictionary<long, List<string>>();
            var lines = File.ReadAllLines("lines.txt");
            var currentBitmask = InitMemory("X");

            foreach (var l in lines) {
            	var bitmask = GetBitmask(l);
            	var command = GetCommand(l);

            	if (bitmask != null) {
            		currentBitmask = bitmask;
            	}

            	if (command.location != -1) {
            		var address = Convert
    						.ToString(command.location, 2)
    						.PadLeft(36, '0');
    				address = address.Substring(address.Length - 36, 36);
            		var result = GeneratePermutations(ApplyBitmask(address.ToCharArray().Select(x => $"{x}").ToList(), currentBitmask));

            		foreach (var r in result) {
            			long loc = Convert.ToInt64(string.Join("", r), 2);
            			memory[loc] = command.bitmask;
            		}
            	}
            }

            long total = 0;
            foreach (var k in memory.Keys) {
        		var m = memory[k];
        		total += Convert.ToInt64(string.Join("", m), 2);
            }

            Console.WriteLine($"Total is {total}");
        }
    }
}
