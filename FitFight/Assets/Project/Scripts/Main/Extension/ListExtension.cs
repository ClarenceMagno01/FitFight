using System;
using System.Collections.Generic;

namespace _Project.Scripts.Main.Extension
{
    public static class ListExtension
    {
        static Random rng = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1); 
                (list[i], list[j]) = (list[j], list[i]); 
            }
            return list;
        }
    }
}