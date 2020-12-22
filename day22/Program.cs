using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace day22 {
    class Program {
		static int gameNumber = 0;

    	static int Recurse(List<int> player1, List<int> player2) {
    		int pWinner = 0;
    		Hashtable record = new Hashtable();

    		gameNumber++;

			while (player1.Count > 0 && player2.Count > 0) {
				var rk = $"{string.Join(",", player1)}-{string.Join(",", player2)}";
            	if (record.ContainsKey(rk)) {
            		return 1;
            	}

        		record[rk] = 1;

            	var next1 = player1.First();
            	var next2 = player2.First();
            	player1.RemoveAt(0);
        		player2.RemoveAt(0);

            	var roundWinner = 0;

            	if (next1 <= player1.Count && next2 <= player2.Count) {
            		var sub1 = player1.Take(next1).ToList();
            		var sub2 = player2.Take(next2).ToList();

        			roundWinner = Recurse(sub1, sub2);
            	} else {
					if (next1 > next2) {
	            		roundWinner = 1;
	            	} else if (next2 > next1) {
	            		roundWinner = 2;
	        		}
            	}

            	if (roundWinner == 1) {
            		player1.Add(next1);
            		player1.Add(next2);
            	} else if (roundWinner == 2) {
            		player2.Add(next2);
            		player2.Add(next1);
        		}
            }

            if (pWinner == 0) {
            	pWinner = player1.Count > 0 ? 1 : 2;
            }

            return pWinner;
    	}

        static void Main(string[] args) {
            var player1 = File.ReadAllLines("player1.txt").Select(x => int.Parse(x)).ToList();
            var player2 = File.ReadAllLines("player2.txt").Select(x => int.Parse(x)).ToList();

            var pWinner = Recurse(player1, player2);

            var winner = pWinner == 1 ? player1 : player2;

        	int tot = 0;
        	for (var i = 0; i < winner.Count; i++) {
        		tot += winner[i] * (winner.Count - i);
        	}

        	Console.WriteLine($"Score is {tot}");
        }
    }
}
