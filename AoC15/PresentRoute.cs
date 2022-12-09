using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day3
{
    // This exercise was funny because I got to know how broken struct/object comparison is implemented
    // When using functions like "Distinct" on custom structs or object, C# compares different objects using 
    // hashcodes, but the hashcode of a struct or object is taken from the first field. 
    //
    // I had to implement a custom comparer and use it along Distinct to overcome that. 

    internal class HousePosition
    {
        public int x;
        public int y;
    }

    internal class HousePositionComparer : IEqualityComparer<HousePosition>
    { 
        public bool Equals(HousePosition? a, HousePosition? b) 
        {
            if (a is not null && b is not null)
                return (a.x == b.x) && (a.y == b.y);
            else
                return false;
        }

        public int GetHashCode(HousePosition obj) 
        {
            string S = obj.x.ToString() + "-" + obj.y.ToString();
            return S.GetHashCode();
        }
    }

    internal class PresentRoute
    {
        List<HousePosition> visitedPositions;
        public int VisitedHouses;

        public PresentRoute(string route, int part)
        {
            visitedPositions = new();

            if (part == 1)
                followRoute(route);
            else
                followRouteRoboSanta(route);

            VisitedHouses = visitedPositions.Distinct(new HousePositionComparer()).Count();
        }

        void followRoute(string route)
        {
            var currentPosition = new HousePosition() { x = 0, y = 0 };

            visitedPositions.Add(currentPosition);

            for(int i=0;i<route.Length;i++) 
            {
                char move = route[i];
                currentPosition = Move(move, currentPosition);
                visitedPositions.Add(currentPosition);
            }
        }

        void followRouteRoboSanta(string route)
        {
            var santaPosition = new HousePosition() { x = 0, y = 0 };
            var roboSantaPosition = new HousePosition() { x = 0, y = 0 };

            visitedPositions.Add(santaPosition);
            visitedPositions.Add(roboSantaPosition);

            for (int i = 0; i < route.Length; i++)
            {
                char move = route[i];
                if (i % 2 == 0)
                {
                    santaPosition = Move(move, santaPosition);
                    visitedPositions.Add(santaPosition);
                }
                else
                {
                    roboSantaPosition = Move(move, roboSantaPosition);
                    visitedPositions.Add(roboSantaPosition);
                }
            }
        }

        static HousePosition Move(char move, HousePosition current) => move switch
        {
            '>' => new HousePosition() { x = current.x + 1, y = current.y},
            '<' => new HousePosition() { x = current.x - 1, y = current.y},
            '^' => new HousePosition() { x = current.x , y = current.y + 1},
            'v' => new HousePosition() { x = current.x , y = current.y - 1},
            _ => throw new ArgumentException("Invalid move char : " + move),
        };
    }
}
