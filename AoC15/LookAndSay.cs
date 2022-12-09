using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15
{
    internal class LookAndSay
    {
        // Ridiculous performance improvement by using StringBuilder and 
        // chars to count (instead of string and int like I did before). 
        public string Play(string input)
        {
            StringBuilder newString = new StringBuilder("");
            char currentChar = input[0];
            char counter = '1';
            for(int i=1;i<input.Length;i++) 
            {
                if (currentChar == input[i])
                    counter++;
                else
                {
                    newString.Append(counter);
                    newString.Append(currentChar);
                    currentChar = input[i];
                    counter='1';
                }
            }
            newString.Append(counter);
            newString.Append(currentChar);
            
            return newString.ToString(); 
        }

        public int PlayTimes(string input, int count)
        {
            string tmp = input;
            for (int i = 0; i < count; i++)
                tmp = Play(tmp);

            return tmp.Length;
        }

    }
}
