using System;
using System.Collections.Generic;

namespace _Project.Scripts.Main.Utilities.Scripts
{
    public class UniqueRandomGenerator
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates a list of unique random integers within a specified range using a partial Fisher-Yates shuffle.
        /// </summary>
        /// <param name="count">The number of unique random integers to generate.</param>
        /// <param name="min">The inclusive lower bound of the range.</param>
        /// <param name="max">The exclusive upper bound of the range.</param>
        /// <returns>A list of unique random integers within the range [min, max).</returns>
        /// <exception cref="ArgumentException">Thrown when count is greater than the number of available unique values in the range.</exception>
        public static List<int> GetUniqueRandomNumbers(int count, int min, int max)
        {
            int upperBound = max - 1;
            int range = upperBound - min + 1;
            if (count > range)
                throw new ArgumentException("Count exceeds the number of unique values in the range.");

            List<int> pool = new List<int>(range);
            for (int i = 0; i < range; i++)
                pool.Add(min + i);

            // Fisher-Yates partial shuffle
            for (int i = 0; i < count; i++)
            {
                int swapIndex = _random.Next(i, range);
                (pool[i], pool[swapIndex]) = (pool[swapIndex], pool[i]);
            }

            return pool.GetRange(0, count);
        }
    }

}