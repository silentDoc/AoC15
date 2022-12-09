using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC15.Day7
{
    enum WireOperations
    { 
        assign,
        bypass,
        not,
        and,
        numeric_and,
        or,
        lshift, 
        rshift
    }

    class wire
    {
        public string name;
        public ushort value;
        public bool hasValue;
        WireOperations howToSolve;

        ushort shiftParameter = 0;
        string nodeA = "";
        string nodeB = "";

        public wire() 
        { 
            name = "";
            hasValue = false;
        }

        public wire(string name, WireOperations howToSolve, ushort value, string nodeA, string nodeB)
        {
            this.name = name;
            this.howToSolve = howToSolve;

            hasValue = (howToSolve == WireOperations.assign);
            this.value = value;
            this.nodeA = nodeA;
            this.nodeB = nodeB;
            shiftParameter = value;
        }

        public void Update(WireOperations howToSolve, ushort value, string nodeA, string nodeB)
        {
            this.howToSolve = howToSolve;
            hasValue = (howToSolve == WireOperations.assign);
            this.value = value;
            this.nodeA = nodeA;
            this.nodeB = nodeB;
            shiftParameter = value;
        }

        public ushort GetValue(List<wire> wires)
        {
            if (hasValue)
                return value;

            wire? wireA = wires.Where(x => x.name == nodeA).FirstOrDefault();
            wire? wireB = wires.Where(x => x.name == nodeB).FirstOrDefault();

            ushort valueA = (wireA != null) ? wireA.GetValue(wires) : (ushort)0;
            ushort valueB = (wireB != null) ? wireB.GetValue(wires) : (ushort)0;

            value = howToSolve switch
            {
                WireOperations.bypass => valueA,
                WireOperations.not => (ushort)(ushort.MaxValue - valueA),
                WireOperations.and => (ushort)(valueA & valueB),
                WireOperations.numeric_and => (ushort)(value & valueA),
                WireOperations.or => (ushort)(valueA | valueB),
                WireOperations.lshift => (ushort)(valueA << shiftParameter),
                WireOperations.rshift => (ushort)(valueA >> shiftParameter),
                _ => throw new Exception("Unsupported operation"),
            };

            hasValue = true;
            return value;
        }
    }

    internal class CircuitManager
    {

        List<wire> wires; 

        int part = 1;
        public CircuitManager(int part)
        {
            this.part = part;
            wires = new();
        }

        wire? getWire(string wireName)
        {
            return wires.Where(x => x.name == wireName).FirstOrDefault();
        }

        public void BuildCircuit(List<string> lines)
        {
            foreach (var line in lines)
                ParseInstruction(line);
        }


        void ParseInstruction(string line)
        {
            var parts = line.Split(" -> ");
            var wire = parts.Last();
            var command = parts[0];
            var factors = parts[0].Split(" ");

            var regex_assign = new Regex(@"^(\w+)");
            var regex_not = new Regex(@"^NOT (\w+)");
            var regex_3factor = new Regex(@"^(\w+) (AND|OR|LSHIFT|RSHIFT) (\w+)"); 

           
            if (regex_not.IsMatch(command))
                wires.Add(new wire(wire, WireOperations.not, 0, factors[1], ""));
            else if (regex_3factor.IsMatch(command))
            {
                if (factors[1] == "AND")
                    if (char.IsDigit(factors[0][0]))
                        wires.Add(new wire(wire, WireOperations.numeric_and, ushort.Parse(factors[0]), factors[2], ""));
                    else
                        wires.Add(new wire(wire, WireOperations.and, 0, factors[0], factors[2]));

                if (factors[1] == "OR")
                        wires.Add(new wire(wire, WireOperations.or, 0, factors[0], factors[2]));

                if (factors[1] == "LSHIFT")
                    wires.Add(new wire(wire, WireOperations.lshift, ushort.Parse(factors[2]), factors[0], ""));

                if (factors[1] == "RSHIFT")
                    wires.Add(new wire(wire, WireOperations.rshift, ushort.Parse(factors[2]), factors[0], ""));
            }
            else if (regex_assign.IsMatch(command))
                if (char.IsDigit(factors[0][0]))
                    wires.Add(new wire(wire, WireOperations.assign, ushort.Parse(factors[0]), "", ""));
                else
                    wires.Add(new wire(wire, WireOperations.bypass, 0, factors[0], ""));

        }

        public ushort GetWireValue(string wireName)
        { 
            var wire = getWire(wireName);


            var result = (wire != null) 
                        ? wire.GetValue(wires)
                        : throw new ArgumentException("Can't find wire : " + wireName);
            
            return result;
        }

        public void OverrideWire(string name, WireOperations operation, ushort value, 
                                 string nodeA, string nodeB)
        {
            var wire = getWire(name);
            if(wire==null)
                throw new ArgumentException("Can't find wire " + name);
            wire.Update(operation, value, nodeA, nodeB);
        }

    }


}
