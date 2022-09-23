using DevExpress.Mvvm;
using DistrictSupplySolution.DbnTables;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.DistrictObjects.BuildingObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
namespace DistrictSupplySolution.DistrictObjects
{
    public class Substation:BindableBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set;}
        public ObservableCollection<OptimizationDataBuilding> OptimizationDataBuildings { get; set; }
        public bool IsLengthsCompleted { get; set; }
       /// <summary>
       /// Получить список комбинаций для декартова произведения
       /// </summary>
       /// <param name="transormerNum">число ТП у підстанції</param>
       /// <param name="transformerPOwer">Номінальна потужність ТП</param>
       /// <param name="minCoefOfLoad">Мінімальний кеф завантаження</param>
       /// <param name="maxCoefOfLoad">Макс коеф завантаження</param>
       /// <param name="MaxLength">Максимальна довжина кабелю</param>
       /// <returns>Список комбінацій для ДПМ</returns>
    public List<List<int>> DefineCombinationsPerSubstation(int transormerNum, int transformerPOwer, double minCoefOfLoad, double maxCoefOfLoad, double MaxLength)
    {
            // Отсортировать номера и мощности в коллекцию по параметру длина
            var FitLengthsColl = from ab in OptimizationDataBuildings
                                 where ab.CableLength <= MaxLength
                                 select new { Number = ab.PlanNumber, Power = ab.FullPower };
            
            int minimalNumber = 0;
            int maximalNum = 0;
            double minimalLoadComparer = transformerPOwer * transormerNum * (minCoefOfLoad + 0.1);
            double maximalLoadComparer = transformerPOwer * transormerNum * (maxCoefOfLoad + 0.2);
            double minimalLoad = transformerPOwer * transormerNum * (minCoefOfLoad+0.1);
            double maximalLoad = transformerPOwer * transormerNum * (maxCoefOfLoad+0.2);  
            var MinimalDetermine = from ab in FitLengthsColl
                                   orderby ab.Power descending
                                   select ab;
            var MaximalDetermine = from ab in FitLengthsColl
                                   orderby ab.Power ascending
                                   select ab;
            foreach (var ab in MinimalDetermine)
            {
                if (ab.Power < minimalLoad)
                {
                    if (minimalLoad >= ab.Power)
                    {
                        minimalLoad -= ab.Power;
                        minimalNumber++;
                    }
                }
                else break;
            }
           foreach (var ab in MaximalDetermine)

            {
                if (ab.Power < maximalLoad)
                {
                    if (maximalLoad >= ab.Power)
                    {
                        maximalLoad -= ab.Power;
                        maximalNum++;
                    }
                }
                else break;
            }
          
            var tolists = from ab in MaximalDetermine select ab.Number;
            IEnumerable<int[]> Combinations = ExtMethods.GetAbCombinations(tolists.ToList()).Where(
                x=>x.Length >= minimalNumber && x.Length<=maximalNum);
            List<List<int>> result = new List<List<int>>();
            foreach (var i in Combinations)
            {
                double loadOfSet = 0;
                foreach(var o in i)
                {
                    OptimizationDataBuilding x = OptimizationDataBuildings.Where(
                        ob =>ob.PlanNumber == o).First();
                    loadOfSet += x.FullPower;
                }
                if (minimalLoadComparer<=loadOfSet&&maximalLoadComparer>=loadOfSet)
                result.Add(i.OrderByDescending(o => o).ToList());
            }
            result = result.Distinct().ToList();
            return result;
    }

    }

}
