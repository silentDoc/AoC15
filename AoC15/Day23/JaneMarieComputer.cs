using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day23
{
    class Instruction
    {
        string command = "";
        string reg = "";
        int offset;
        int index;

        public (int newIndex, int newValue) Run(Dictionary<string, int> registers)
            =>  command switch
                {
                    "hlf" => (index + 1, registers[reg] = registers[reg] / 2),
                    "tpl" => (index + 1, registers[reg] = registers[reg] * 3),
                    "inc" => (index + 1, registers[reg] = registers[reg] + 1),
                    "jmp" => (index + offset, 0),
                    "jie" => (index + ((registers[reg] % 2 == 0) ? offset : 1), 0),
                    "jio" => (index + (registers[reg] == 1 ? offset : 1), 0),
                    _ => throw new Exception("Invalid command")
                };

        public Instruction(string line, int index)
        {
            this.index = index;
            var groups = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            command = groups[0].Trim();

            if (command == "jmp")
                offset = int.Parse(groups[1].Trim());
            else
                reg = groups[1][0].ToString();

            if (command == "jie" || command == "jio")
                offset = int.Parse(groups[2].Trim());
        }
    }


    internal class JaneMarieComputer
    {
        Dictionary<string, int> registers = new();
        List<Instruction> program = new();

        public void ParseInput(List<string> lines)
        {
            for(int row = 0; row < lines.Count;row++)
                program.Add(new Instruction(lines[row], row));
        }
        
        int RunProgram(int part = 1)
        {
            registers["a"] = 0;
            registers["b"] = 0;
            int currentIndex = 0;
            int value = 0;

            while (currentIndex < program.Count)
            {
                (currentIndex, value) = program[currentIndex].Run(registers);
            }
            return registers["b"];
        }

        public int Solve(int part = 1)
            => RunProgram(part);
    }
}
