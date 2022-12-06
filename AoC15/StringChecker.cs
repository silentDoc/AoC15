using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15
{
    internal class StringChecker
    {
        string Str;
        public bool IsNice = false;

        public StringChecker(string str) 
        {
            Str = str;
            bool vowels = AtLeast3Vowels();
            bool digits = DoubleDigits();
            bool substr = WithoutSubstrings();
            IsNice = AtLeast3Vowels() && DoubleDigits() && WithoutSubstrings();
        }

        bool AtLeast3Vowels()
        {
            return (Str.Where(x => "aeiouAEIOU".IndexOf(x) != -1).Count() >= 3);
        }

        bool DoubleDigits()
        { 
            for(int i = 0; i<Str.Length-1;i++)
                if (Str[i]== Str[i+1])
                    return true;
            return false;
        }
        bool WithoutSubstrings()
        {
            return (Str.IndexOf("ab") == -1)
                    && (Str.IndexOf("cd") == -1)
                    && (Str.IndexOf("pq") == -1)
                    && (Str.IndexOf("xy") == -1);
        }
    }
}
