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
        public ObservableCollection<AbstractBuilding> AbstractBuildings { get; set; }
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
            var FitLengthsColl = from ab in AbstractBuildings
                                 where ab.CableLength <= MaxLength
                                 select new { Number = ab.PlanNumber, Power = ab.FullPower };
            
            int minimalNumber = 0;
            int maximalNum = 0;
            double minimalLoad = transformerPOwer * transormerNum * minCoefOfLoad;
            double maximalLoad = transformerPOwer * transormerNum * maxCoefOfLoad;

            var MinimalDetermine = from ab in FitLengthsColl
                                   orderby ab.Power ascending
                                   select ab;
           foreach (var ab in MinimalDetermine)
            {
                if (0 < minimalLoad)
                {
                    minimalLoad -= ab.Power;
                    minimalNumber++;
                }
                if (0 < maximalLoad)
                {
                    maximalLoad -= ab.Power;
                    maximalNum++;
                }
                else break;
            }
            //var results = ExtentionMethods.GetCombinations(input).Where(x 
            //=> x.Length >= minNumOfBuildings && x.Length <= maxNumOfBuildings);
            var tolists = from ab in MinimalDetermine select ab.Number;
            var Combinations = ExtMethods.GetAbCombinations(tolists.ToList()).Where(x
                => x.Length >= minimalNumber && x.Length<=maximalNum);


            return default; //TODO
    }

    }

}
