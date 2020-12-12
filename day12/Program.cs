using System;
using System.IO;
using System.Linq;

namespace day12 {
    class Program {
    	static (string action, int amount) GenerateTuple(string input) {
    		return (input.Substring(0, 1), int.Parse(input.Substring(1)));
    	}

        static void Main(string[] args) {
            var instructions = File.ReadAllLines("lines.txt").Select(x => GenerateTuple(x));
            (int x, int y) location = (0, 0);
            (int x, int y) waypoint = (10, 1);

            foreach (var i in instructions) {
            	switch (i.action) {
            		case "N":
            			waypoint.y += i.amount;
            			break;
            		case "S":
            			waypoint.y -= i.amount;
            			break;
            		case "E":
            			waypoint.x += i.amount;
            			break;
            		case "W":
            			waypoint.x -= i.amount;
            			break;
            		case "L":
            			int rotations = i.amount / 90;

            			for (var num = 0; num < rotations; num++) {
            				int newX = waypoint.x;
            				int newY = waypoint.y;

        					newX = -waypoint.y;
        					newY = waypoint.x;

            				waypoint.x = newX;
            				waypoint.y = newY;
            			}

            			break;
            		case "R":
            			int totalRots = i.amount / 90;

            			for (var num = 0; num < totalRots; num++) {
            				int newX = waypoint.x;
            				int newY = waypoint.y;

        					newX = waypoint.y;
        					newY = -waypoint.x;

            				waypoint.x = newX;
            				waypoint.y = newY;
            			}

            			break;
            		case "F":
            			location.x += waypoint.x * i.amount;
            			location.y += waypoint.y * i.amount;

            			break;

            	}
            }

            Console.WriteLine($"As the crow {Math.Abs(location.x) + Math.Abs(location.y)}");
        }
    }
}
