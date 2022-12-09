using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC15.Day5
{
    internal class StringChecker
    {
        string Str;
        public bool IsNice = false;

        public StringChecker(string str, int part) 
        {
            Str = str;
            IsNice = (part ==1) 
                     ? AtLeast3Vowels() && DoubleDigits() && WithoutSubstrings()
                     : TwoLettersAtLeastTwice() && OneLetterBetweenRepeat();
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

        bool TwoLettersAtLeastTwice()
        {
            bool repeatsTwice = false;
            bool overlap = false;

            for (int i = 0; i < Str.Length - 1; i++)
            {
                var substring = Str.Substring(i, 2);
                Regex r = new(substring);
                var matches = r.Matches(Str).ToList();
                
                if(matches.Count()>1)
                    repeatsTwice = true;

                var indices = matches.Select(x => x.Index).OrderBy(x => x).ToList();
                
                for(int j = 1; j<indices.Count; j++)
                    if (indices[j] - indices[j-1] == 1)
                        overlap = true;
            }

            return !overlap && repeatsTwice;
        }

        bool OneLetterBetweenRepeat()
        {
            for (int i = 0; i < Str.Length - 2; i++)
                if (Str[i] == Str[i + 2])
                    return true;
            return false;
        }
    }
}
