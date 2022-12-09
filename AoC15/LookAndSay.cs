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
        public string Play(string input)
        {
            string newString = "";
            char currentChar = input[0];
            int counter = 1;
            for(int i=1;i<input.Length;i++) 
            {
                if (currentChar == input[i])
                    counter++;
                else
                {
                    newString += (counter.ToString() + currentChar);
                    currentChar = input[i];
                    counter=1;
                }
            }
            newString += (counter.ToString() + currentChar);
            return newString; 
        }

        public int PlayTimes(string input, int count)
        {
            string tmp = input;
            for (int i = 0; i < count; i++)
            {
                tmp = Play(tmp);
                Trace.WriteLine(i.ToString());
            }


            return tmp.Length;
        }

    }
}
