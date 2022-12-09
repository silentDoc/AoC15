using AoC15.Day9;
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

    class TableSetup
    {
        public string arrangement;
        public int totalHappiness;

        public TableSetup(string arrangement, int happiness)
        {
            this.arrangement = arrangement;
            this.totalHappiness = happiness;
        }
    }

    internal class DinnerTable
    {
        List<Pair> combinations;
        List<string> guests;
        List<int> totalHappiness;
        List<TableSetup> arrangements;

        public DinnerTable(List<string> pairings)
        {
            combinations = new();
            guests = new();
            totalHappiness = new();
            arrangements = new();

            processInput(pairings);
        }

        void processInput(List<string> pairings)
        {
            foreach (var pair in pairings)
            {
                var str = pair;
                str = str.Replace("would gain ", "");
                str = str.Replace("would lose ", "-");
                str = str.Replace("happiness units by sitting next to ", "");
                str = str.Replace(".", "");

                var parts = str.Split(" ");
                var guest = parts[0];
                var happiness = int.Parse(parts[1]);
                var guestBeside = parts[2];

                combinations.Add(new Pair(guest, guestBeside, happiness));
                guests.Add(guest);
                guests.Add(guestBeside);
            }
            guests = guests.Distinct().ToList();
        }

        void PossibleSeat(string guests, List<string> availablePeople, string arrangement)
        {
            if (availablePeople.Count == 0)
                solveHappiness(arrangement);

            foreach (var person in availablePeople)
                PossibleSeat(person, availablePeople.Where(x => x != person).ToList(), arrangement + "," + person);
        }

        void solveHappiness(string arrangement)
        {
            var people = arrangement.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var tableHappiness = 0;

            for (int i = 1; i < people.Length - 1; i++)
            {
                tableHappiness += combinations.Where(x => x.guest == people[i] && x.guestBeside == people[i + 1]).Select(x => x.happiness).FirstOrDefault();
                tableHappiness += combinations.Where(x => x.guest == people[i] && x.guestBeside == people[i - 1]).Select(x => x.happiness).FirstOrDefault();
            }
            // add edges
            tableHappiness += combinations.Where(x => x.guest == people[0] && x.guestBeside == people[1]).Select(x => x.happiness).FirstOrDefault();
            tableHappiness += combinations.Where(x => x.guest == people[0] && x.guestBeside == people.Last()).Select(x => x.happiness).FirstOrDefault();

            tableHappiness += combinations.Where(x => x.guest == people.Last() && x.guestBeside == people[0]).Select(x => x.happiness).FirstOrDefault();
            tableHappiness += combinations.Where(x => x.guest == people.Last() && x.guestBeside == people[people.Length - 2]).Select(x => x.happiness).FirstOrDefault();

            totalHappiness.Add(tableHappiness);
            arrangements.Add(new TableSetup(arrangement, tableHappiness));
        }

        public int GetHappiness(int part)
        {
            foreach (var guest in guests)
                PossibleSeat(guest, guests.Where(x => x != guest).ToList(), guest);

            return (part == 1) ? totalHappiness.Max()
                               : totalHappiness.Min();
        }
    }
}

    
