using System;
using System.Collections.Generic;
using System.Linq;


namespace SortingLib
{
    public static class GameUtil
    {
        /// <summary>
        /// Generates a list of random integers.
        /// </summary>
        /// <param name="number">The number of integers.</param>
        /// <param name="minimum">Lowest value (including).</param>
        /// <param name="maximum">Highest value (including).</param>
        public static int[] GenerateList(int number, int minimum = 1, int maximum = 100)
        {
            var rand = new Random();
            var list = new int[number];

            for (var i = 0; i < number; )
            {
                var cand = rand.Next(minimum, maximum + 1);
                if (list.Take(i).Contains(cand)) continue;
                list[i] = cand;
                i++;
            }

            return list;
        }


        /// <summary>
        /// Sorts the list and returns the steps taken.
        /// </summary>
        public static IEnumerable<SortingStep> Sort(int[] list, SortingAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case SortingAlgorithms.BubbleSort:
                    return BubbleSort.Sort(list);

                case SortingAlgorithms.MergeSort:
                    return MergeSort.Sort(list);

                default:
                    throw new ArgumentException($"Unknown algorithm '{algorithm}'.");
            }
        }
    }
}
