using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15
{
    internal class Elevator
    {
        public int FinalFloor;
        int startingFloor = 0;
        string elevatorSequence;

        public Elevator(string sequence)
        {
            elevatorSequence = sequence;
            int up = sequence.Where(x => x == '(').Count();
            int down = sequence.Length - up;

            FinalFloor = startingFloor + up - down;
        }

        public int BasementEntry()
        {
            int currentFloor = startingFloor;
            int pos = 0;
            while (pos < elevatorSequence.Length)
            {
                currentFloor += elevatorSequence[pos] == '(' ? 1 : -1;
                if (currentFloor < 0)
                    break;
                pos++;
            }
            return pos + 1;
        }
    }
}
