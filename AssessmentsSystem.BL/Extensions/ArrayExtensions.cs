using System;

namespace AssessmentsSystem.BL.Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[] array, Random random = default)
        {
            var rng = random ?? new Random();
            var n = array.Length;

            while (n > 1)
            {
                var k = rng.Next(n--);

                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}