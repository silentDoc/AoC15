using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15
{
    internal class StringMemory
    {
        List<int> codeLens = new();
        List<int> memoLens = new();
        
        public StringMemory() 
        {
        }

        public int Process(List<string> input)
        {
            foreach (var line in input)
            {
                // C# regexp does not unescape it like we would like
                var tst = Regex.Unescape(line);
                Trace.WriteLine(tst.Length.ToString());  // :(

                var strClean = line.Substring(1, line.Length - 2).Replace("\\\"", "\"");
                strClean = strClean.Replace("\\\\", "?");
                strClean = Regex.Replace(strClean, @"\\x[0-9a-f]{2}", "?");

                codeLens.Add(line.Length);
                memoLens.Add(strClean.Length);
            }

            return codeLens.Sum() - memoLens.Sum();
        
        }


    }
}
