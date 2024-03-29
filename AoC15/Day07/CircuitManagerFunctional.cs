﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Copied with pride from  https://markheath.net/post/advent-of-code-day7 - so all kudos to Mark Heath
// I liked this approach a lot, so I took this to study and improve. 
// The approach is the same I came up with, but much more elegantly implemented and taking
// more advantage of c# funcs

namespace AoC15.Day07
{
    internal class CircuitManagerFunctional
    {
        Dictionary<string, string[]> instructions = new Dictionary<string, string[]>();

        public CircuitManagerFunctional()
        {
        }

        public void BuildCircuit(List<string> input)
        {
            instructions = input.Select(x => x.Split(" ")).ToDictionary(x => x.Last());
        }

        public ushort GetWireValue(string wire)
        {
            // Tiny func that will tell us if we have a value or look recurively until we can solve the value
            Func<string, ushort> eval = x =>
                    char.IsLetter(x[0]) ? GetWireValue(x) : ushort.Parse(x);

            Func<string[], ushort> assign = x => eval(x[0]);
            Func<string[], ushort> and = x => (ushort)(eval(x[0]) & eval(x[2]));
            Func<string[], ushort> or = x => (ushort)(eval(x[0]) | eval(x[2]));
            Func<string[], ushort> lshift = x => (ushort)(eval(x[0]) << eval(x[2]));
            Func<string[], ushort> rshift = x => (ushort)(eval(x[0]) >> eval(x[2]));
            Func<string[], ushort> not = x => (ushort)~eval(x[1]);

            var wireOperands = instructions[wire];

            ushort value;
            if (wireOperands[1] == "->") value = assign(wireOperands);
            else if (wireOperands[1] == "AND") value = and(wireOperands);
            else if (wireOperands[1] == "OR") value = or(wireOperands);
            else if (wireOperands[1] == "LSHIFT") value = lshift(wireOperands);
            else if (wireOperands[1] == "RSHIFT") value = rshift(wireOperands);
            else if (wireOperands[0] == "NOT") value = not(wireOperands);
            else throw new InvalidDataException("Unrecognised command");

            // Once the wire is solved, we update it with the value to speed up future references
            instructions[wire] = new string[] { value.ToString(), "->", wire };
            return value;
        }
    }
}
