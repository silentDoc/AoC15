using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15
{
    internal class PasswordGenerator
    {
        char[] forbiddenChars = { 'i', 'o', 'l' };
        
        bool rule_straight(string pwd)
        {
            for (int i = 0; i < pwd.Length - 2; i++)
                if ((pwd[i + 1] - pwd[i] == 1) && (pwd[i + 2] - pwd[i + 1] == 1))
                    return true;

            return false;
        }

        bool rule_letters(string pwd)
            => (pwd.IndexOfAny(forbiddenChars) == -1);

        bool rule_pairs(string pwd)
        {
            List<string> matches = new();
            int l = pwd.Length-1;

            if ((pwd[0] == pwd[1]) && (pwd[1] != pwd[2]))   // start
                matches.Add(pwd.Substring(0,2));

            for (int i = 1; i < l - 1; i++)                 // middle
                if ((pwd[i - 1] != pwd[i]) && (pwd[i] == pwd[i + 1]) && (pwd[i + 1] != pwd[i + 2]))
                    matches.Add(pwd.Substring(i, 2));

            if ((pwd[l] == pwd[l-1]) && (pwd[l-1] != pwd[l-2])) // end
                matches.Add(pwd.Substring(l - 1, 2));

            return  (matches.Count < 2) ? false
                                        : matches.Where(x => x != matches[0]).Count()>0;
        }

        string increment(string currentPass, int pos)
        {
            StringBuilder newPass = new StringBuilder(currentPass);
            char current = currentPass[pos];

            char newChar = '\0';

            if (current == 'z')
            {
                newPass[pos] = 'a';
                return increment(newPass.ToString(), pos - 1);
            }

            newChar = (char)(current + 1);
            newPass[pos] = newChar;

            if (forbiddenChars.Contains(newChar))
            {
                // Skip a forbidden char, set all the trail to a 
                newChar = (char)(newChar + 1);
                for (int j = pos + 1; j < currentPass.Length; j++)
                    newPass[j] = 'a';
            }
            newPass[pos] = newChar;

            return newPass.ToString();
        }

        public bool CheckPass(string pwd)
           => rule_letters(pwd) && rule_pairs(pwd) && rule_straight(pwd);

        public string FindNextPass(string currentPass)
        {
            int pos = currentPass.Length - 1;
            var newPass = increment(currentPass, pos);

            while(!CheckPass(newPass))
                newPass = increment(newPass, pos);

            return newPass;
        }
    }
}
