using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day4
{
    internal class AdventCoinMiner
    {
        string key;
        public AdventCoinMiner(string privateKey)
        {
            key = privateKey;
        }

        public int Mine(int part)
        {
            string startSequence = (part == 1) ? "00000" : "000000";

            int i = 0;
            while(true) 
            {
                string str = key + i.ToString();
                var hexMD5 = CreateMD5(str);
                if (hexMD5.StartsWith(startSequence))
                    break;
                i++;
            }

            return i;
        }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); 
            }
        }
    }
}
