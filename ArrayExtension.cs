using System;

namespace _20112020dz
{
    public static class ArrayExtension
    {
        public static T[] Mix<T>(this T[] array)
        {
            var rand = new Random(DateTime.Now.Millisecond);

            for (var i = 0; i < array.Length; i++)
            {
                var r = rand.Next(array.Length);

                var temp = array[r];
                array[r] = array[i];
                array[i] = temp;
            }

            return array;
        }
    }
}