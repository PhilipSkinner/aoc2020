using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day21 {
	class Food {
		public List<string> Ingredients { get; set; }
		public List<string> Allergens { get; set; }

		public Food(string line) {
			var parts = line.Replace(")", "").Split("(contains ");

			this.Ingredients = parts[0].Split(" ").Select(x => x.Trim()).Where(x => x != "").ToList();
			this.Allergens = parts[1].Split(",").Select(x => x.Trim()).Where(x => x != "").ToList();
		}
	}

    class Program {
    	static Dictionary<string, string> Lookup { get; set; }
    	static List<string> Ignore { get; set; }

    	static Dictionary<string, int> GetPossibilities(List<Food> foods) {
    		var ret = new Dictionary<string, int>();

    		foreach (var f in foods) {
    			foreach (var a in f.Allergens) {
    				if (!Lookup.ContainsKey(a)) {
    					if (!ret.ContainsKey(a)) {
    						ret[a] = 0;
    					}

    					ret[a]++;
    				}
    			}
    		}

    		return ret;
    	}

    	static Dictionary<string, int> GetIngredientsForAllergen(string allergen, List<Food> foods) {
    		var ret = new Dictionary<string, int>();
    		var exists = Ignore.Concat(Lookup.Values.ToList()).ToList();

    		foreach (var f in foods) {
    			if (f.Allergens.IndexOf(allergen) != -1) {
    				f.Ingredients.Where(i => exists.IndexOf(i) == -1).ToList().ForEach(i => {
    					if (!ret.ContainsKey(i)) {
    						ret[i] = 0;
    					}
    					ret[i]++;
					});
    			}
    		}

    		return ret;
    	}

    	static List<string> GetSingleIngredients (List<Food> foods) {
    		var ret = new Dictionary<string, int>();
    		var exists = Lookup.Values.ToList();

    		foreach (var f in foods) {
    			foreach (var i in f.Ingredients.Where(i => exists.IndexOf(i) == -1)) {
    				if (!ret.ContainsKey(i)) {
    					ret[i] = 0;
    				}

    				ret[i]++;
    			}
    		}

    		return ret.Keys.ToList().Where(x => ret[x] == 1).ToList();
    	}

        static void Main(string[] args) {
        	Lookup = new Dictionary<string, string>();
        	Ignore = new List<string>();

        	var data = File.ReadAllLines("data.txt").Select(x => new Food(x)).ToList();

        	var poss = GetPossibilities(data);

        	var singles = GetSingleIngredients(data);

        	Console.WriteLine($"Got {singles.Count}");

        	int i = 0;

        	while (poss.Keys.Count > 0) {
        		Console.WriteLine($"{poss.Keys.Count} possibilities - {Ignore.Count} ignored");

        		foreach (var k in poss.Keys) {
        			//get the possible ingredients
        			var ingredients = GetIngredientsForAllergen(k, data);

        			var allKeys = ingredients.Keys.ToList();

        			if (allKeys.Count == 1) {
        				Console.WriteLine($"Found one");
        				Lookup[k] = allKeys[0];
    				} else {
						var matches = allKeys.Where(x => ingredients[x] > 1).ToList();

						var max = 0;

						foreach (var m in matches) {
							if (ingredients[m] > max) {
								max = ingredients[m];
							}
						}

						matches = allKeys.Where(x => ingredients[x] == max).ToList();

	        			if (matches.Count == 1) {
	        				//found one
	        				Console.WriteLine($"Found one");
	        				Lookup[k] = matches[0];
	        			} else {
	        				var canIgnore = allKeys.Where(x => ingredients[x] == 1).ToList();

        					canIgnore.ForEach(ignore => {
        						if (Ignore.IndexOf(ignore) == -1) {
        							Ignore.Add(ignore);
								}
    						});

        					Console.WriteLine($"Too many match more than 1 - {matches.Count}");
	        			}
    				}
        		}

        		i++;

				poss = GetPossibilities(data);
        	}

        	int count = 0;
        	var allIngredients = Lookup.Values.ToList();
        	foreach (var f in data) {
        		count += f.Ingredients.Sum(x => allIngredients.IndexOf(x) == -1 ? 1 : 0);
        	}

        	Console.WriteLine($"{count}");
        	Console.WriteLine($"{string.Join(",", Lookup.Keys.ToList().OrderBy(x => x).Select(x => Lookup[x]))}");

        }
    }
}
