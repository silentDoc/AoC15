using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day14
{
    class Reindeer
    {
        public string name ="";
        public int flyTime;
        public int flySpeed;
        public int restTime;

        public Reindeer(string name, int flyTime, int restTime, int flySpeed)
        { 
            this.name = name;
            this.flyTime = flyTime;
            this.restTime = restTime;
            this.flySpeed = flySpeed;
        }


        public int totalTime 
        {
            get { return flyTime + restTime; }
        }

        public int Race(int seconds)
            => ((seconds / totalTime) * flySpeed * flyTime) + Math.Min(seconds % totalTime, flyTime) * flySpeed;
    }

    internal class ReindeerRacer
    {
        List<Reindeer> racers;
        public ReindeerRacer(List<string> input)
        {
            racers = new();
            foreach (var item in input)
            {
                //Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.
                string str = item;
                str = str.Replace("can fly ", "").Replace("km/s for ", "");
                str = str.Replace("seconds, but then must rest for ", "").Replace(" seconds.", "");

                var parts = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                racers.Add( new Reindeer(parts[0], int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[1])) );
            }

        }

        public int Race(int seconds) =>
             racers.Select(x => x.Race(seconds)).Max();

        public int RaceNewSystem(int seconds)
        {
            int[] points = new int[racers.Count];

            for (int i = 1; i <= seconds; i++)
            {
                var snapshot = racers.Select(x => x.Race(i)).ToList();
                
                int leadDist = snapshot.Max();
                var award = snapshot.Select(x => x == leadDist).ToList();

                for (int j = 0; j < snapshot.Count; j++)
                    if (award[j]) points[j]++;

            }

            return points.Max();
        }
    }
}
