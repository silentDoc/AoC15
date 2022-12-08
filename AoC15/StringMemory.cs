using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15
{
    internal class StringMemory
    {
        string unescape(string escaped)
        {
            string strClean = escaped.Substring(1, escaped.Length - 2).Replace("\\\"", "\"").Replace(@"\\", @"\");
            return Regex.Replace(strClean, @"\\x[0-9a-f]{2}", "_"); // Use whatever to replace, it will take 1 char
        }

        string escape(string unescaped) =>
             "\"" + unescaped.Replace(@"\", @"\\").Replace("\"", "\\\"") + "\"";


        public int Process(List<string> input) =>
            input.Select(x => new { original = x, processed = unescape(x) })
                .Sum(y => y.original.Length - y.processed.Length);

        public int ProcessP2(List<string> input) => 
            input.Select(x => new { original = x, processed = escape(x) })
                .Sum(y => y.processed.Length - y.original.Length);
    }
}
