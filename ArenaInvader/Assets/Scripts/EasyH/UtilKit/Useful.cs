using System.Collections.Generic;
using UnityEngine;

namespace EasyH.UtilKit {
    
    public class Useful
    {
        public static List<T> Combination<T>(T[] array, int count)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException(nameof(array));
            }

            if (count < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(count));
            }

            if (count > array.Length)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(count),
                    "count cannot be greater than array length.");
            }

            T[] buffer = (T[])array.Clone();
            List<T> retval = new List<T>(count);

            for (int i = buffer.Length - 1, j = 0; j < count; i--, j++)
            {
                int rand = Random.Range(0, i + 1);

                retval.Add(buffer[rand]);
                (buffer[i], buffer[rand]) = (buffer[rand], buffer[i]);

            }

            return retval;
        }

    }
}
