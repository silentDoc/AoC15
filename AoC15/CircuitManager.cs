using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15
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
            // Ugly as it gets - first thing to refactor
            Regex regex_Assign = new(@"^[0-9]+ -> \b[a-zA-Z]{1,2}\b");
            Regex regex_Bypass = new(@"^\b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_Not = new(@"^NOT \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_And = new(@"^\b[a-zA-Z]{1,2}\b AND \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_NumericAnd = new(@"^[0-9]+ AND \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_Or = new(@"^\b[a-zA-Z]{1,2}\b OR \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_Rshift = new(@"^\b[a-zA-Z]{1,2}\b RSHIFT \b[0-9]{1,2}\b -> \b[a-zA-Z]{1,2}\b");
            Regex regex_Lshift = new(@"^\b[a-zA-Z]{1,2}\b LSHIFT \b[0-9]{1,2}\b -> \b[a-zA-Z]{1,2}\b");

            if (regex_Not.IsMatch(line))
                ProcessNot(line);
            else if (regex_And.IsMatch(line))
                ProcessAnd(line);
            else if (regex_NumericAnd.IsMatch(line))
                ProcessNumericAnd(line);
            else if (regex_Or.IsMatch(line))
                ProcessOr(line);
            else if (regex_Rshift.IsMatch(line))
                ProcessRShift(line);
            else if (regex_Lshift.IsMatch(line))
                ProcessLShift(line);
            else if (regex_Assign.IsMatch(line))
                ProcessAssign(line);
            else if (regex_Bypass.IsMatch(line))
                ProcessBypass(line);
            else
                throw new ArgumentException("Can't parse line : " + line);
        }

        void ProcessAssign(string line)
        {
            // @"[0-9]+ -> \b[a-zA-Z]{1,2}\b
            var elements = line.Replace(" -> ", " ").Split(" ");
            var value = ushort.Parse(elements[0]);
            var name = elements[1];

            wires.Add(new wire(name, WireOperations.assign, value, "", ""));
        }

        void ProcessBypass(string line)
        {
            // @"[0-9]+ -> \b[a-zA-Z]{1,2}\b
            var elements = line.Replace(" -> ", " ").Split(" ");
            var nodeA = elements[0];
            var name = elements[1];

            wires.Add(new wire(name, WireOperations.bypass, 0, nodeA, ""));
        }

        void ProcessNot(string line)
        {
            // @"NOT \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace("NOT ", "");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var wireA = elements[0];
            var name = elements[1];

            wires.Add(new wire(name, WireOperations.not, 0, wireA, ""));
            return;
        }

        void ProcessAnd(string line)
        {
            //@"\b[a-zA-Z]{1,2}\b AND \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace(" AND ", " ");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var wireA = elements[0];
            var wireB = elements[1];
            var name = elements[2];

            wires.Add(new wire(name, WireOperations.and, 0, wireA, wireB));
            return;
        }

        void ProcessNumericAnd(string line)
        {
            //@"[0-9]+ AND \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace(" AND ", " ");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var value = ushort.Parse(elements[0]);
            var wireA = elements[1];
            var name = elements[2];

            wires.Add(new wire(name, WireOperations.numeric_and, value, wireA, ""));
            return;
        }

        void ProcessOr(string line)
        {
            // @"\b[a-zA-Z]{1,2}\b OR \b[a-zA-Z]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace(" OR ", " ");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var wireA = elements[0];
            var wireB = elements[1];
            var name = elements[2];

            wires.Add(new wire(name, WireOperations.or, 0, wireA, wireB));
            return;
        }

        void ProcessRShift(string line)
        {
            // @"\b[a-zA-Z]{1,2}\b RSHIFT \b[0-9]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace(" RSHIFT ", " ");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var wireA = elements[0];
            var value = ushort.Parse(elements[1]);
            var name = elements[2];

            wires.Add(new wire(name, WireOperations.rshift, value, wireA, ""));
            return;
        }

        void ProcessLShift(string line)
        {
            // @"\b[a-zA-Z]{1,2}\b LSHIFT \b[0-9]{1,2}\b -> \b[a-zA-Z]{1,2}\b"
            var str = line.Replace(" LSHIFT ", " ");
            var elements = str.Replace(" -> ", " ").Split(" ");
            var wireA = elements[0];
            var value = ushort.Parse(elements[1]);
            var name = elements[2];

            wires.Add(new wire(name, WireOperations.lshift, value, wireA, ""));
            return;
        }

        public ushort GetWireValue(string wireName)
        { 
            var wire = getWire(wireName);


            var result = (wire != null) 
                        ? wire.GetValue(wires)
                        : throw new ArgumentException("Can't find wire : " + wireName);
            
            return result;
        }

    }


}
