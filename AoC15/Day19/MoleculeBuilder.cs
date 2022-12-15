using AoC15.Day17;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15.Day19
{
    record struct Pair
    {
        public string element;
        public string replacement;
    }
    internal class MoleculeBuilder
    {
        List<Pair> pairs= new List<Pair>();
        string molecule = "";
        public MoleculeBuilder(List<string> input, int part = 1)
        {
            
            foreach (var line in input)
            {
                if (line == "")
                    break;
                var split = line.Split(" => ");
                pairs.Add(new Pair { element = split[0], replacement = split[1] });
            }

            molecule = input.Last();
        }

        public int SolvePart1()
        {
            List<string> molecules = new();
            pairs.ForEach( x => molecules.AddRange( GetAllCombinations(x, molecule)));
            return molecules.Distinct().Count();
        }

        List<string> GetAllCombinations(Pair pair, string molecule)
        {
            Regex regex = new Regex(pair.element);
            var matches = regex.Matches(molecule);
            List<string> combinations= new List<string>();
            
            for(int i=0; i<matches.Count; i++)
                combinations.Add( regex.Replace(molecule, pair.replacement, 1, matches[i].Index) );

            return combinations;
        }
    }
}
