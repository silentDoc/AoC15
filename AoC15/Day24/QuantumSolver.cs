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


        long FindQuantumEntanglement()
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

        // The approach for part 2 is not very elegant - I just added an ident level for the 4th group (auch)
        // I have a nice todo to optimize this one.
        long FindQuantumEntanglement_withTrunk()
        {
            long targetBalance = elements.Sum() / 4;
            List<long> quantumEntanglements = new();
            var firstGroupList = FindGroup(elements, new List<long>(), targetBalance).ToList();

            // Only consider combinations with the number of elements equal or less to the totalElements/3
            firstGroupList = firstGroupList.Where(x => x.Count < (elements.Count / 4)).ToList();
            firstGroupList = firstGroupList.OrderBy(x => x.Count).ToList();

            // Keep a list of minimum QEs by element count, to quickly discard combinations that are not better than what we already have
            Dictionary<int, long> quantumValues = new();
            for (int i = 1; i < (elements.Count / 4 + 2); i++)
                quantumValues[i] = long.MaxValue;


            // Now the problem becomes finding a levelled 3-way combination with the remaining elements
            foreach (var firstGroup in firstGroupList)
            {

                var firstGroupCount = firstGroup.Count;
                var firstGroupQE = firstGroup.Aggregate(1, (long acc, long val) => acc * val);

                var currentMin = quantumValues[firstGroup.Count];

                if (firstGroupQE >= currentMin)
                    continue;

                var rest_of_the_list = elements.ToList();
                firstGroup.ForEach(x => rest_of_the_list.Remove(x));

                var rest_of_the_list_Count = rest_of_the_list.Count;
                
                var secondGroupList = FindGroup(rest_of_the_list, new List<long>(), targetBalance).ToList();
                if (secondGroupList.Count == 0)
                    continue;

                foreach (var secondGroup in secondGroupList)
                {
                    var secondGroupCount = secondGroup.Count;

                    // The first group has to have the fewest elements
                    if (firstGroupCount <= secondGroupCount)
                    {
                        var rest_of_the_list2 = rest_of_the_list.ToList();
                        secondGroup.ForEach(x => rest_of_the_list2.Remove(x));
                        var thirdGroupList = FindGroup(rest_of_the_list2, new List<long>(), targetBalance).ToList();
                        if (thirdGroupList.Count == 0)
                            continue;
                        foreach (var thirdGroup in thirdGroupList)
                        {
                            var thirdGroupCount = thirdGroup.Count;
                            var finalGroupCount = elements.Count - firstGroupCount-secondGroupCount - thirdGroupCount;

                            if( firstGroupCount <= thirdGroupCount && firstGroupCount<= finalGroupCount)
                            { 
                                quantumEntanglements.Add(firstGroupQE);
                                quantumValues[firstGroup.Count] = firstGroupQE;
                            }
                        }
                    }
                }
            }

            return quantumEntanglements.Min();
        }


        public long Solve(int part = 1)
            => (part==1) ? FindQuantumEntanglement() : FindQuantumEntanglement_withTrunk();
    }
}
