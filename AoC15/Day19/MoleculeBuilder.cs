using AoC15.Day17;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics;
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

        List<string> GetAllCombinations(Pair pair, string molecule)
        {
            Regex regex = new Regex(pair.element);
            var matches = regex.Matches(molecule);
            List<string> combinations= new List<string>();
            
            for(int i=0; i<matches.Count; i++)
                combinations.Add( regex.Replace(molecule, pair.replacement, 1, matches[i].Index) );

            return combinations;
        }

        int countStr(string mol, string part)
        {
            var count = 0;
            for (var index = mol.IndexOf(part); index >= 0; index = mol.IndexOf(part, index + 1), ++count) { }
            return count;
        }

        public int Solve(int part = 1)
            => (part == 1) ? SolvePart1() : SolvePart2();
            //=> (part == 1) ? SolvePart1() : SolvePart2_M2();

        public int SolvePart1()
        {
            List<string> molecules = new();
            pairs.ForEach(x => molecules.AddRange(GetAllCombinations(x, molecule)));
            return molecules.Distinct().Count();
        }

        // The second part is a grammar
        //  Analyzing the input, we can see that all the rules follow some form like:
        //  α => βγ
        //  α => βRnγAr
        //  α => βRnγYδAr
        //  α => βRnγYδYεAr
        //
        //  Rn, Ar, and Y are only on the left side of the equation - we can reduce the problem to 
        //
        //  #NumSymbols - #Rn - #Ar - 2 * #Y - 1
        //
        //  Subtract of #Rn and #Ar because those are just extras.
        //  Subtract two times #Y because we get rid of the Ys and the extra elements following them.
        //  Subtract one because we start with "e".
        public int SolvePart2()
        {
            var num = molecule.Count(char.IsUpper) - countStr(molecule, "Rn") - countStr(molecule, "Ar") - 2 * countStr(molecule, "Y") - 1;
            return num;
        }

        string Contract(string molec)
        {
            foreach (var entry in pairs)
            {
                int len = entry.replacement.Length;
                int pos = molec.LastIndexOf(entry.replacement);

                if (pos != -1)
                    return molec.Substring(0, pos) + entry.element + molec.Substring(pos + len);
            }
            return "";
        }
      

        // A more programatic approach to part 2 - not fully working yet
        public int SolvePart2_M2()
        {
            int steps = 0;
            string mol = molecule;
            while (mol != "" && mol != "e")
            {
                mol = Contract(mol);
                steps++;
            }
            return steps;
        }
    }
}
