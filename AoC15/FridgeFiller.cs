using AoC15.Day3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day17
{
    internal class FridgeContainer
    {
        public string name;
        public int capacity;
    }

    internal class FridgeContainerListComparer : IEqualityComparer<List<FridgeContainer>>
    {
        public bool Equals(List<FridgeContainer>? a, List<FridgeContainer>? b)
        {
            if (a is not null && b is not null)
            {
                if (a.Count() != b.Count())
                    return false;

                var aa = a.OrderBy(x => x.name).ToList();
                var bb = b.OrderBy(x => x.name).ToList();

                for (int i = 0; i < aa.Count(); i++)
                    if (aa[i].name != bb[i].name)
                        return false;

                return true;
            }
            else
                return false;
        }

        public int GetHashCode(List<FridgeContainer> obj)
        {
            StringBuilder sb = new();
            obj.OrderBy(x=> x.name).ToList().ForEach(x => sb.Append(x.name));
            string s = sb.ToString();
            return s.GetHashCode();
        }
    }

    public class FridgeFiller
    {
        int fridgeSpace;
        List<FridgeContainer> fridgeContainers = new();

        public FridgeFiller(int fridgeSpace, List<string> input)
        {
            this.fridgeSpace = fridgeSpace;
            char name_char = 'A';   // We will generate a different name for each container we load
            int name_int = 1;

            foreach (var line in input)
            {
                fridgeContainers.Add(new FridgeContainer() { capacity = int.Parse(line), name = name_char + name_int.ToString() });
                name_char++;
                name_int++;
            }
        }

        public int SolvePart1()
        {
            var combs = putInFridge(fridgeContainers, new List<FridgeContainer>()).ToList();
            var uniqueCombinations = combs.Distinct(new FridgeContainerListComparer()).ToList();
            return uniqueCombinations.Count();
        }

        public int SolvePart2()
        {
            combinations.Clear();
            var combs = putInFridge(fridgeContainers, new List<FridgeContainer>()).ToList();
            var uniqueCombinations = combs.Distinct(new FridgeContainerListComparer()).ToList();
            var minNumContainers = uniqueCombinations.Select(p => p.Count).Min();
            var minUniqueCombinations = uniqueCombinations.Where(p => p.Count == minNumContainers).Count();

            return minUniqueCombinations;
        }


        IEnumerable<List<FridgeContainer>> putInFridge(List<FridgeContainer> availableContainers, List<FridgeContainer> usedContainers)
        {
            var sum = usedContainers.Sum(x => x.capacity);
            var remaining = fridgeSpace - sum;

            for (int n = 0; n < availableContainers.Count; n++)
            { 
                var candidate = availableContainers[n];
                if (candidate.capacity > remaining)
                    continue;
                var newList = usedContainers.ToList();
                newList.Add(candidate);
                if (candidate.capacity == remaining)
                { 
                    yield return newList.OrderBy(x => x.name).ToList(); 
                }
                else
                {
                    var rest = availableContainers.Skip(n + 1).ToList();
                    foreach (var d in putInFridge(rest, newList))
                    {
                        yield return d.OrderBy(x => x.name).ToList();
                    }
                }
            }
        }
    }
}
