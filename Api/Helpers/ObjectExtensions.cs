using System;
using System.Collections.Generic;
using System.Linq;

namespace StackgipInventory.Helpers
{
    public static class ObjectExtensions
    {
        public static bool IsNull<T>(this T source)
        {
            return source == null;
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == default;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> act)
        {
            foreach (var i in array)
            {
                act(i);
            }
               
            return array;
        }
        public static string[] SplitStringList(this string fileNames)
        {
            if (string.IsNullOrWhiteSpace(fileNames) || string.IsNullOrEmpty(fileNames))
            {
                return new string[] { };
            }

            return fileNames.Split(',');
        }
    }
}
