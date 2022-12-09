using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC15
{
    internal class JSONHelper
    {
        public int GetSum(string jsonStr)
        {
            Regex numRegex = new Regex(@"[\-0-9]+");
            var matches = numRegex.Matches(jsonStr);

            return matches.Select(x => int.Parse(x.Value)).Sum();
        }
    }
        
}
