using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public int GetSumJson(string input, int part = 2)
        { 
            var json = JObject.Parse(input);
            return (int) sumJson(json);
        }


        // This SO question came handly
        // https://stackoverflow.com/questions/53649570/how-do-you-check-if-a-json-array-is-an-array-of-objects-or-primitives-recursivel
        long sumJson(JToken token)
        {
            if (token is JObject jobj)
                return jobj.Properties().Select(p => p.Value).Sum(jt => sumJson(jt));
            else if (token is JArray jarr)
                return jarr.Sum(x => sumJson(x));
            else if (token is JValue jval)
                return (jval.Value is long) ? (long)jval.Value : 0;

            throw new Exception("Unknown node type");
        }
    }
}
