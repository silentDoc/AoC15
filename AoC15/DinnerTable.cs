using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day13
{
    // Looks like an iteration of the previous TripPlanner Problem, so the approach will be the same
    class Pair
    {
        public string guest;
        public string guestBeside;
        public int happiness;

        public Pair(string guest, string guestBeside, int happiness)
        {
            this.guest = guest;
            this.guestBeside = guestBeside;
            this.happiness = happiness;
        }
    }

    internal class DinnerTable
    {
    }
}
