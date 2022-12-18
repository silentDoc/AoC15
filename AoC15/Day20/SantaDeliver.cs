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
               

        int Deliver(int minNumPresents)
        {
            var houses = new int[minNumPresents / 10 + 1];
            for (int elf = 1; elf < houses.Length; elf++)
                for (int house = elf; house < houses.Length; house += elf)
                    houses[house] = houses[house] + 10 * elf;

            for (int i = 0; i < houses.Length; i++)
                if (houses[i] > minNumPresents)
                    return i;

            return 0;
        }

        int Deliver2(int minNumPresents)
        {
            var houses = new int[minNumPresents / 11 + 1];
            for (int elf = 1; elf < houses.Length; elf++)
            {
                int visits = 0;
                for (int house = elf; (house < houses.Length && visits<=50); house += elf)
                {
                    visits++;
                    houses[house] = houses[house] + 11 * elf;
                }
            }

            for (int i = 0; i < houses.Length; i++)
                if (houses[i] >= minNumPresents)
                    return i;

            return 0;
        }


        int SolvePart1(int minNumPresents)
            => Deliver(minNumPresents);

        int SolvePart2(int minNumPresents)
            => Deliver2(minNumPresents);
    }
}
