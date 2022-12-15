using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day09
{
    class Route
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

    internal class TripPlanner
    {
        List<Route> cityRoutes;
        List<string> cities;
        List<int> distances;

        public TripPlanner(List<string> routes)
        {
            cityRoutes = new();
            cities = new();
            distances = new();

            ProcessInput(routes);
        }

        void ProcessInput(List<string> routes)
        {
            foreach (var route in routes)
            {
                var parts = route.Split(" = ");
                var city1 = parts[0].Split(" to ")[0];
                var city2 = parts[0].Split(" to ")[1];
                int distance = int.Parse(parts[1]);

                cityRoutes.Add(new Route(city1, city2, distance));
                cityRoutes.Add(new Route(city2, city1, distance));

                cities.Add(city1);
                cities.Add(city2);
            }
            cities = cities.Distinct().ToList();
        }

        public void PossibleRoutes(string city, List<string> destinations, int distance)
        {
            if (destinations.Count == 0)
                distances.Add(distance);        // Final node, save the distance

            foreach (var dest in destinations)
            {
                int newDistance = distance + cityRoutes.Where(x => x.origin == city && x.destination == dest).Select(x => x.distance).FirstOrDefault(); ;
                PossibleRoutes(dest, destinations.Where(x => x != dest).ToList(), newDistance);
            }
        }

        public int GetRoute(int part)
        {
            foreach (var city in cities)
                PossibleRoutes(city, cities.Where(x => x != city).ToList(), 0);

            return part == 1 ? distances.Min()
                             : distances.Max();
        }
    }
}
