using System;
using System.IO;
using System.Linq;

namespace day8 {
	public enum InstructionType {
		acc,
		jmp,
		nop
	}

	class Instruction {
		public InstructionType type { get; set; }
		public int arg { get; set; }
		private bool executed { get; set; }

		public Instruction(string type, string args) {
			this.type = Enum.Parse<InstructionType>(type);
			this.arg = int.Parse(args);
			this.executed = false;
		}

		public void Exec(ref int accumulator, ref int location) {
			if (this.executed) {
				throw new Exception("Already executed!");
			}

			this.executed = true;

			if (this.type == InstructionType.acc) {
				accumulator += this.arg;
				location += 1;
			}

			if (this.type == InstructionType.nop) {
				location += 1;
			}

			if (this.type == InstructionType.jmp) {
				location += this.arg;
			}
		}

		public void Reset() {
			this.executed = false;
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllLines("lines.txt").Select(x => x.Split(' ')).Select(x => new Instruction(x[0], x[1])).ToList();

        	foreach (var l in lines) {
        		int location = 0;
        		int accumulator = 0;

        		if (l.type == InstructionType.jmp) {
        			l.type = InstructionType.nop;
        		} else if (l.type == InstructionType.nop) {
        			l.type = InstructionType.jmp;
        		}

        		lines.ForEach(x => x.Reset());

	        	try {
	        		while (location < lines.Count) {
	        			lines[location].Exec(ref accumulator, ref location);
	        		}

	        		Console.WriteLine($"Accumulator value {accumulator}");
	    		} catch(Exception) {

	    		}

	    		if (l.type == InstructionType.jmp) {
        			l.type = InstructionType.nop;
        		} else if (l.type == InstructionType.nop) {
        			l.type = InstructionType.jmp;
        		}
        	}
        }
    }
}
