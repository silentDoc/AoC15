using AoC15.Day24;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day25
{
    internal class CopyProtection
    {
        Dictionary<(int row, int col), long> Codes = new();

        int row = 0;
        int col = 0;
        public void ParseInput(List<string> lines)
        { 
            var line = lines[0];
            line = line.Replace("To continue, please consult the code grid in the manual.  Enter the code at row ", "");
            line = line.Replace(", column ", " ");
            line = line.Replace(".", "");

            var values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            row = int.Parse(values[0]);
            col = int.Parse(values[1]);
        }

        long NextCode(long code)
            => (code * (long)252533) % (long)33554393;

        long FindCode(int part = 1)
        {
            bool found = false;
            Codes[(1, 1)] = 20151125;
            long currentCode = 20151125;
            int newMax = 1;

            while (!found)
            {
                newMax++;
                for (int i = 1; i <= newMax; i++)
                {
                    var i_col = i;
                    var i_row = (newMax + 1) - i;
                    currentCode = NextCode(currentCode);
                    Codes[(i_row, i_col)] = currentCode;

                    if (i_row == row && i_col == col)
                        return currentCode;
                        
                }
            }
            return 0;
        }

        public long Solve(int part = 1)
            => FindCode(part);
    }
}
