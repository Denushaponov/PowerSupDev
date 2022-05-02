using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistrictSupplySolution
{
    public class test
    {
        //   List<List<List<int>>> result { get; set; }
        public List<List<List<int>>> CPS(List<List<List<int>>> sequences)
        {
            int i = 0;
            List<List<List<int>>> result=new List<List<List<int>>>();
            foreach (var sequence in sequences)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Iteration #" + i + " Start");
                var localSequence = sequence;
                if (i == 0) 
                {
                    List<List<int>> tempRes = new List<List<int>>();
                    foreach (var x in sequence)
                    {
                        List<int> item = new List<int>();
                        tempRes.Add(item);
                    }
                    result.Add(tempRes);
                }
                else
                {
                    List<List<List<int>>> tempResult = new List<List<List<int>>>();
                    tempResult = result;
                    result = new List<List<List<int>>>();
                    foreach (var curr in sequence)
                    {
                        foreach (var prev in tempResult)
                        {
                            List<List<int>> temp = new List<List<int>>();
                            List<int> item = new List<int>();
                            temp = prev;
                            temp.Add(curr);
                            var comparer = temp.SelectMany(x => x);
                            if (comparer.GroupBy(x => x).All(g => g.Count() == 1))
                            result.Add(temp);   
                        }
                    }
                    

                }
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Iteration #" + i + " END");
                System.Diagnostics.Debug.WriteLine("Number of elements: "+result.Count());
               i++;
            }
            return result;
        }
    }

    public static class ExtMethods
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
            foreach (var sequence in sequences)
            {
                var localSequence = sequence;
                List<List<List<int>>> combinationsList2 = new List<List<List<int>>>();//
                result = result.SelectMany(
                  _ => localSequence,
                  (seq, item) => seq.Concat(new[] { item }));

                System.Diagnostics.Debug.WriteLine(result.Count() + " Before insc");
                System.Diagnostics.Debug.WriteLine(DateTime.Now);
                //result.Inrersec();/// test code
               // result = result.Inrersec();//test
                System.Diagnostics.Debug.WriteLine(result.Count() + " after Insc");
                System.Diagnostics.Debug.WriteLine(DateTime.Now);
            }
            
            return result;
        }
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

       


        public static IEnumerable<IEnumerable<IEnumerable<T>>> Inrersec<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> sequences)
        {
            
            IEnumerable<IEnumerable<IEnumerable<T>>> result = new[] { Enumerable.Empty<IEnumerable<T>>() };

            // START Alghorithm
            foreach (var district in sequences) 
            {
                var DistrictChanges = district.Take(1).ToList();
                district.Aggregate((last, curr) =>
                {
                    bool contains = false;
                    bool numExists = false;

                    // Добавить сисок Int  и при каждой итерации добавлять в него пердыдущие элементы чтобы потом
                    // проверя ть их на наличие дубликатов
                    // Для каждого елемента n в текущей последовательности
                    foreach (var n in curr)
                    {
                        // Если предідущая поледовательность содержит елемент из текущей
                        // Или номер существует в одной из пердыдущих последоваельностей
                        // То есть Если во время предыдущей итерации выявлено что
                        // Набор содержит дубликат
                        if (last.Contains(n) || numExists == true)
                        {
                            //Содержится дубликат
                            contains = true;
                            // Выйти из цикла чтобы перейти к следующему набору
                            // Последовательности
                            break;
                        }
                        else
                        {
                            // Если верхние уусловия не удовлетворены
                            // дубликата нет
                            contains = false;
                        }

                        // CHECK PREVIOUS SRQUENCES FOR PURPOSE OF DUPLICATES
                        // каждый добавленый в набор список проверяется
                        foreach (var lt in DistrictChanges)
                        {
                            // Если этот список содержит 
                            // елемент цикла верхнего уровня

                            if (lt.Contains(n))
                            {
                                // Число есть в списке, выйти из цикла
                                numExists = true;
                                break;
                            }
                        }
                        // Если число есть в списке выйти из цикла верхнего уровня
                        // Поменяв соответствующий флаг
                        if (numExists == true)
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (contains == false) DistrictChanges.Add(curr);
                    return curr;
                });
                if (DistrictChanges.Count() == district.Count())
                {
                    result = result.Concat(new[] { DistrictChanges });
                }

            }
            // END Alghorithm 
            return result;

        }


        public static IEnumerable<IEnumerable<IEnumerable<T>>> RemoveInsc<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> sequences)
        {

            IEnumerable<IEnumerable<IEnumerable<T>>> result = new[] { Enumerable.Empty<IEnumerable<T>>() };

            // START Alghorithm
            foreach (var district in sequences)
            {

                //      var DistrictChanges = district.Take(1).ToList();

                bool ToAdd = true;
                int i = 0;
                foreach (var b in district)
                {
                    foreach (var c in district)
                    {
                        if (b != c && b.Intersect(c).Any())
                        {
                            ToAdd = false;
                            break;
                        }
                        else i++;
                    }
                    if (ToAdd == false)
                    {
                        break;
                    }
                }
                if (ToAdd == false)
                    result.Concat(new[] { district });
                //
                //district.Aggregate((last, curr) =>
                //    { 

                //        flag = last.Intersect(curr).Any();
                //        if (!flag)
                //        {
                //            DistrictChanges.Add(curr);// fine
                //        }
                //        else { }
                //            return curr;
                //    });
                //   // if (DistrictChanges.Count() == district.Count())
                //  //  {
                //  //      result = result.Concat(new[] { DistrictChanges });
                //  //  }

                //if (flag == false) result.Concat(new[] { district });

                //}
                //// END Alghorithm
            }
           
            return result;
        }

        public static IEnumerable<IEnumerable<IEnumerable<T>>> Opti2<T>(this IEnumerable<IEnumerable<IEnumerable<T>>> sequences)
        {

            IEnumerable<IEnumerable<IEnumerable<T>>> result = new[] { Enumerable.Empty<IEnumerable<T>>() };

            // START Alghorithm
            foreach (var district in sequences)
            {
                bool flag=false;
                    var DistrictChanges = district.Take(1).ToList();


                List<T> bbb = new List<T>();
                district.Aggregate((last, curr) =>
                    {
                        if (flag != true)
                        {
                            foreach (var x in last)
                                bbb.Add(x);
                            flag = last.Intersect(curr).Any() && curr.Intersect(bbb).Any();

                            if (!flag)
                            {
                                DistrictChanges.Add(curr);// fine
                            }
                        }
                      //  else { }
                        return curr;
                    });
                // if (DistrictChanges.Count() == district.Count())
                // {
                //     result = result.Concat(new[] { DistrictChanges });
                //}

                if (flag == false)
                    sequences = sequences.Where(o => o != district);
                   // result.Concat(new[] { district });
                
            }
            // END Alghorithm
        

            return sequences;
        }
      
    }
}
