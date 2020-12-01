using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day1 {
    class Program {
        static void Main(string[] args) {
            //horribly memory inefficient, wanted to solve it with linq though
            var lines = File.ReadAllText("list.txt").Split('\n').Select(x => int.Parse(x));

            var twoParts =
                (
                    from entry in (
                        from first in lines
                        from second in lines
                        select new {first, second}
                    )
                    where entry.first + entry.second == 2020
                    select entry.first * entry.second
                ).First()
            ;

            var threeParts =
                (
                    from entry in (
                        from first in lines
                        from second in lines
                        from third in lines
                        select new {first, second, third}
                    )
                    where entry.first + entry.second + entry.third == 2020
                    select entry.first * entry.second * entry.third
                ).First()
            ;

            Console.WriteLine($"Two components: {twoParts}");
            Console.WriteLine($"Three components: {threeParts}");
        }
    }
}
