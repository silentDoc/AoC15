using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day20
{
    internal class SantaDeliver
    {
        public int Solve(int minNumPresents, int part = 1)
            => part == 1 ? SolvePart1(minNumPresents) : SolvePart2(minNumPresents);

        int SolvePart1(int minNumPresents)
            => Deliver(minNumPresents);

        int CalcNumPresents(int houseNumber)
        { 
            HashSet<int> divisors = new HashSet<int>();
            int n = houseNumber;
            for (int i = 1; i < n / 2; i++)
            {
                if (divisors.Contains(i))
                    continue;
                if (n % i == 0)
                { 
                    divisors.Add(i);
                    divisors.Add(n / i);
                }
            }

            int retVal = 0;
            foreach (var divisor in divisors)
                retVal += divisor * 10;
            return retVal; 
        }

        int Deliver(int minNumPresents)
        {
            var houses = new int[minNumPresents / 10 + 1];
            for (int elf = 1; elf < houses.Length; elf++)
                for (int house = 0; house < houses.Length; house += elf)
                    houses[house] = houses[house] + 10 * elf;

            for (int i = 0; i < houses.Length; i++)
                if (houses[i] > minNumPresents)
                    return i;

            return 0;
        }

        int SolvePart2(int minNumPresents)
        {
            return 0;
        }
    }
}
