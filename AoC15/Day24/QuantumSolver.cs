using AoC15.Day17;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC15.Day24
{
    internal class QuantumSolver
    {
        List<long> elements = new();

        public void ParseInput(List<string> lines)
            => lines.ForEach(line => elements.Add(long.Parse(line.Trim())));


        IEnumerable<List<long>> FindGroup(List<long> availableElements, List<long> usedElements, long targetWeight)
        {
            var sum = usedElements.Sum();
            var remaining = targetWeight - sum;

            for (int n = 0; n < availableElements.Count; n++)
            {
                var candidate = availableElements[n];
                if (candidate > remaining)
                    continue;
                
                var newList = usedElements.ToList();
                newList.Add(candidate);

                if (candidate == remaining)
                    yield return newList.OrderByDescending(x => x).ToList();
                else
                {
                    var rest = availableElements.Skip(n + 1).ToList();
                    foreach (var d in FindGroup(rest, newList, targetWeight))
                    {
                        yield return d.OrderByDescending(x => x).ToList();
                    }
                }
            }
        }



        long FindQuantumEntanglement(int part = 1)
        {
            long targetBalance = elements.Sum() / 3;
            List<long> quantumEntanglements = new();
            var quantumCombinations = FindGroup(elements, new List<long>(), targetBalance).ToList();
            
            // Only consider combinations with the number of elements equal or less to the totalElements/3
            quantumCombinations = quantumCombinations.Where(x => x.Count < (elements.Count/3)).ToList();
            quantumCombinations = quantumCombinations.OrderBy(x => x.Count).ToList();

            // Keep a list of minimum QEs by element count, to quickly discard combinations that are not better than what we already have
            Dictionary<int, long> quantumValues = new();
            for (int i = 1; i < (elements.Count / 3 + 2); i++)
                quantumValues[i] = long.MaxValue;


            foreach (var combination in quantumCombinations)
            {
                var first_group_count = combination.Count;
                var quantum_entanglement = combination.Aggregate(1, (long acc, long val) => acc * val);

                var currentMin = quantumValues[combination.Count];

                if (quantum_entanglement > currentMin)
                    continue;

                var remainingList = elements.ToList();
                combination.ForEach(x => remainingList.Remove(x));
                
                var remainingListCount = remainingList.Count;
                var remainingCombinations = FindGroup(remainingList, new List<long>(), targetBalance).ToList();

                if (remainingCombinations.Count == 0)
                    continue;

                foreach (var remaining_combination in remainingCombinations)
                {
                    var second_group_count = remaining_combination.Count;
                    var third_group_count = remainingListCount - second_group_count;
                    // The first group has to have the fewest elements
                    if ( first_group_count <= second_group_count && first_group_count <= third_group_count)
                    {
                        quantumEntanglements.Add(quantum_entanglement);
                        quantumValues[combination.Count] = quantum_entanglement;
                    }
                }
            }

            return quantumEntanglements.Min();
        }


        public long Solve(int part = 1)
            => FindQuantumEntanglement(part);
    }
}
