﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15.Day16
{
    public static class AuntSueFinder
    {
        static Dictionary<string, int> theSue = new();

        static void loadSue()
        {
            theSue.Clear();
            theSue["children"] = 3;
            theSue["cats"] = 7;
            theSue["samoyeds"] = 2;
            theSue["pomeranians"] = 3;
            theSue["akitas"] = 0;
            theSue["vizslas"] = 0;
            theSue["goldfish"] = 5;
            theSue["trees"] = 3;
            theSue["cars"] = 2;
            theSue["perfumes"] = 1;
        }

        static List<Dictionary<string, int>> getSues(List<string> input)
        {
            List<Dictionary<string, int>> allSues = new();
            // Sample -  Sue 1: goldfish: 6, trees: 9, akitas: 0
            var regex = new Regex(@"Sue ([0-9]\d*): (\w+): ([0-9]\d*), (\w+): ([0-9]\d*), (\w+): ([0-9]\d*)");

            foreach (string line in input)
            {
                Dictionary<string, int> dict = new();
                var groups = regex.Match(line).Groups;
                dict[groups[2].Value] = int.Parse(groups[3].Value);
                dict[groups[4].Value] = int.Parse(groups[5].Value);
                dict[groups[6].Value] = int.Parse(groups[7].Value);

                allSues.Add(dict);
            }
            return allSues;
        }

        public static int Part1(List<string> input)
        {
            loadSue();
            var allSues = getSues(input);
            int found = 1;

            foreach (var sue in allSues)
            {
                var keys = sue.Keys.ToList();
                var foundSue = keys.Aggregate(true, (acc, val) => acc && (sue[val] == theSue[val]));
                if (foundSue)
                {
                    found = allSues.IndexOf(sue) + 1;
                    break;
                }
            }

            return found;
        }

        public static int Part2(List<string> input)
        {
            loadSue();
            var allSues = getSues(input);
            int found = 1;

            List<string> greater = new() { "cats", "trees"};
            List<string> fewer = new() {"pomeranians", "goldfish"};

            foreach (var sue in allSues)
            {
                var keys = sue.Keys.ToList();
                bool foundSue = true;
                foreach (var key in keys)
                {
                    if (greater.Contains(key))
                        foundSue = foundSue && (sue[key] > theSue[key]);
                    else if (fewer.Contains(key))
                        foundSue = foundSue && (sue[key] < theSue[key]);
                    else
                        foundSue = foundSue && (sue[key] == theSue[key]);
                }

                if (foundSue)
                {
                    found = allSues.IndexOf(sue) + 1;
                    break;
                }
            }

            return found;
        }
    }
}
