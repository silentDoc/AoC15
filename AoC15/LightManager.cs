using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15
{
    internal class LightManager
    {
        // Using jagged arrays instead of multidimensionals to use Linq and fp
        int[][] panel = null;
        int part = 1;

        public LightManager(int part)
        {
            this.part = part;

            panel = new int[1000][];
            for (int i = 0; i < 1000; panel[i++] = new int[1000]) ;
            
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    panel[i][j] = 0;

        }

        public int CountLights()
        {
            int suma = 0;

            for (int i = 0; i < 1000; i++)
                suma += panel[i].Sum();

            return suma;
        }

        public void DoInstruction(string line)
        {
            // ugly
            
            if (line.StartsWith("turn on "))
                TurnOn(line.Replace("turn on ", ""));
            else if (line.StartsWith("turn off "))
                TurnOff(line.Replace("turn off ", ""));
            else if (line.StartsWith("toggle "))
                Toggle(line.Replace("toggle ", ""));
            else
                throw new ArgumentException("Can't manage instruction : " + line);
        }

        void TurnOn(string range)
        {
            var pos = GetPositions(range);
            if(part==1)
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                        panel[i][j] = 1;
            else
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                        panel[i][j]++;
        }

        void TurnOff(string range)
        {
            var pos = GetPositions(range);
            if (part == 1)
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                        panel[i][j] = 0;
            else
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                    {
                        panel[i][j]-= (panel[i][j]>0)?1:0;
                    }

        }
        void Toggle(string range)
        {
            var pos = GetPositions(range);
            if (part == 1)
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                        panel[i][j] = (panel[i][j] == 0) ? 1 : 0;
            else
                for (var i = pos[0]; i <= pos[2]; i++)
                    for (var j = pos[1]; j <= pos[3]; j++)
                        panel[i][j] += 2;
        }

        int[] GetPositions(string range)
        {
            var str = range.Replace("through ","");
            var positions = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var start = positions[0].Split(",");
            var stop = positions[1].Split(",");

            var result = new int[4];
            result[0] = int.Parse(start[0]);
            result[1] = int.Parse(start[1]);
            result[2] = int.Parse(stop[0]);
            result[3] = int.Parse(stop[1]);

            return result;

        }

    }
}
