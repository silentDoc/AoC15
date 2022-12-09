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
        public string origin;
        public string destination;
        public int distance;

        public Route(string origin, string destination, int distance)
        {
            this.origin = origin;
            this.destination = destination;
            this.distance = distance;
        }
    }
    internal class DinnerTable
    {
    }
}
