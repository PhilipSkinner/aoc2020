using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day4 {
	class Passport {
		public int? birthYear { get; set; }
		public int? issueYear { get; set; }
		public int? expirationYear { get; set; }
		public string height { get; set; }
		public string hairColour { get; set; }
		public string eyeColour { get; set; }
		public string passportId { get; set; }
		public int? countryId { get; set; }

		private readonly Regex hairColourCheck = new Regex(@"^#[0-9a-f]{6}$");
		private readonly Regex eyeColourCheck = new Regex(@"^amb|blu|brn|gry|grn|hzl|oth$");
		private readonly Regex passportIdCheck = new Regex(@"^\d{9}$");

		public Passport(string entry) {
			foreach (var e in entry.Replace("\n", " ").Replace("  ", " ").Split(' ')) {
				var components = e.Split(':');

				switch (components[0]) {
					case "byr":
						this.birthYear = int.Parse(components[1]);
						break;
					case "iyr":
						this.issueYear = int.Parse(components[1]);
						break;
					case "eyr":
						this.expirationYear = int.Parse(components[1]);
						break;
					case "hgt":
						this.height = components[1];
						break;
					case "hcl":
						this.hairColour = components[1];
						break;
					case "ecl":
						this.eyeColour = components[1];
						break;
					case "pid":
						this.passportId = components[1];
						break;
					case "cid":
						this.countryId = int.Parse(components[1]);
						break;
				}
			}
		}

		public bool IsBirthYearValid() {
			return
				this.birthYear.HasValue
				&& this.birthYear.Value >= 1920
				&& this.birthYear.Value <= 2002;
		}

		public bool IsIssueYearValid() {
			return
				this.issueYear.HasValue
				&& this.issueYear.Value >= 2010
				&& this.issueYear.Value <= 2020;
		}

		public bool IsExpirationYearValid() {
			return
				this.expirationYear.HasValue
				&& this.expirationYear >= 2020
				&& this.expirationYear <= 2030;
		}

		public bool IsHeightValid() {
			if (
				string.IsNullOrWhiteSpace(this.height)
				|| (
					this.height.IndexOf("cm") == -1
					&& this.height.IndexOf("in") == -1
				)
			) {
				return false;
			}

			int hNum = 0;
			if (!int.TryParse(this.height.Replace("cm", "").Replace("in", ""), out hNum)) {
				return false;
			}

			return (
				this.height.IndexOf("cm") != -1
				&& hNum >= 150 && hNum <= 193
			) || (
				this.height.IndexOf("in") != -1
				&& hNum >= 59 && hNum <= 76
			);
		}

		public bool IsHairColourValid() {
			return
				!string.IsNullOrWhiteSpace(this.hairColour)
				&& this.hairColourCheck.IsMatch(this.hairColour);
		}

		public bool IsEyeColourValid() {
			return
				!string.IsNullOrWhiteSpace(this.eyeColour)
				&& this.eyeColourCheck.IsMatch(this.eyeColour);
		}

		public bool IsPassportIdValid() {
			return
				!string.IsNullOrWhiteSpace(this.passportId)
				&& this.passportIdCheck.IsMatch(this.passportId);
		}

		public bool IsValid() {
			return
				this.IsBirthYearValid()
				&& this.IsIssueYearValid()
				&& this.IsExpirationYearValid()
				&& this.IsHeightValid()
				&& this.IsHairColourValid()
				&& this.IsEyeColourValid()
				&& this.IsPassportIdValid();
		}
	}

    class Program {
        static void Main(string[] args) {
        	var lines = File.ReadAllText("lines.txt").Split("\n\n").Select(x => new Passport(x)).ToList();

        	Console.WriteLine($"Total: {lines.Count()}");
        	Console.WriteLine($"Number valid: {lines.Where(x => x.IsValid()).Count()}");
        }
    }
}
