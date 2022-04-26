using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistrictSupplySolution
{
    public static class ExtMethods
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
        //var results = ExtentionMethods.GetCombinations(input).Where(x => x.Length >= minNumOfBuildings && x.Length <= maxNumOfBuildings);
        // fine thing this makes combinations 
        public static IEnumerable<T[]> GetAbCombinations<T>(List<T> source)
        {
            for (var i = 0; i < (1 << source.Count); i++)
                yield return source
                   .Where((t, j) => (i & (1 << j)) != 0)
                   .ToArray();
        }
    }
}
