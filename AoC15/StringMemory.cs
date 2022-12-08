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
        List<int> codeLens = new();
        List<int> memoLens = new();
        List<int> encodedLens = new();
        
        public StringMemory() 
        {
        }

        string unescape(string escaped)
        {
            string strClean = escaped.Substring(1, escaped.Length - 2);
            strClean = strClean.Replace("\\\"", "\"");
            strClean = strClean.Replace(@"\\", @"\");   
            strClean = Regex.Replace(strClean, @"\\x[0-9a-f]{2}", "_"); // Use whatever to replace, it will take 1 char
            return strClean;
        }

        string escape(string unescaped)
        {
            string strEnc = unescaped.Replace(@"\", @"\\");
            strEnc = strEnc.Replace("\"", "\\\"");
            strEnc = "\"" + strEnc + "\"";
            
            return strEnc;
        }



        public int Process(List<string> input)
        {
            foreach (var line in input)
            {
                // C# regexp does not unescape it like we would like
                //var tst = Regex.Unescape(line);
                //Trace.WriteLine(tst.Length.ToString());  // :(

                var strClean = unescape(line);

                codeLens.Add(line.Length);
                memoLens.Add(strClean.Length);
            }

            return codeLens.Sum() - memoLens.Sum();
        }

        public int ProcessPart2(List<string> input)
        {
            foreach (var line in input)
            {
                var encoded = escape(line);

                codeLens.Add(line.Length);
                encodedLens.Add(encoded.Length);
            }

            return encodedLens.Sum() - codeLens.Sum();
        }


    }
}
