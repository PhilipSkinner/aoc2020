using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace day25 {
    class Program {
    	static long multiplier = 20201227;

        static void Main(string[] args) {
			long cardPK = 17773298;
			long doorPK = 15530095;

			long v = 1;
			long loopSize = 0;
			while (v != cardPK) {
				//7 is a prime, check out how to construct 5764801 from primes only - 7^8
    			v = (v * 7) % multiplier;
    			loopSize++;
			}

			Console.WriteLine($"Loop size is {loopSize}");

			long encryption = 1;
			for (long i = 0; i < loopSize; i++) {
    			encryption = (encryption * doorPK) % multiplier;
			}

			Console.WriteLine($"Result is {encryption}");
        }
    }
}