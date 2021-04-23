using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WpfPaging.DistrictObjects;

namespace DistrictSupplySolution.DistrictObjects.BuildingObjects
{
    /// <summary>
    /// Жилые здания как одно
    /// </summary>
    public class UnitedApartmentBuildings
    {
        // Создаю коллекцию жилых зданий для хранения
        // поле принимает коллекцию зданий из микрорайона


        public ObservableCollection<ApartmentBuilding> ApartmentBuildings { get; set; } = new ObservableCollection<ApartmentBuilding>();

        /// <summary>
        /// Поле принимает в себя поля всех зданий первой категории 
        /// </summary>
       

        public ObservableCollection<UnitedApartmentBuilding> UnitedApartmentBuildingsCollection { get; set; } = new ObservableCollection<UnitedApartmentBuilding>();

       

        /// <summary>
        /// 
        /// </summary>
        public void GetUnitedApartmentBuildings()
        {
            if (UnitedApartmentBuildingsCollection != null) UnitedApartmentBuildingsCollection.Clear(); 
             
            
            UnitedApartmentBuildingsCollection.Add(new UnitedApartmentBuilding());
            UnitedApartmentBuildingsCollection.Add(new UnitedApartmentBuilding());
           
            foreach (var ab in ApartmentBuildings)
            {
                if (ab.ElectrificationLevel==1)
                {
                    
                    if (UnitedApartmentBuildingsCollection[0].PlanNumber == null)
                        UnitedApartmentBuildingsCollection[0].PlanNumber = ab.PlanNumber.ToString();
                    else
                        UnitedApartmentBuildingsCollection[0].PlanNumber =    UnitedApartmentBuildingsCollection[0].PlanNumber +", "+ ab.PlanNumber.ToString();

                    // Перенимает наибольее значение єтажей для определения коефициентов спроса
                    if (ab.Levels >      UnitedApartmentBuildingsCollection[0].Levels)
                             UnitedApartmentBuildingsCollection[0].Levels = ab.Levels; 

                    // присваивает значение категории 
                         UnitedApartmentBuildingsCollection[0].ElectrificationLevel = ab.ElectrificationLevel;

                    // копирует из каждой коллеуции 
                    foreach (var el in ab.PowerPlants.Elevators)
                    {
                             UnitedApartmentBuildingsCollection[0].PowerPlants.Elevators.Add(el);
                    }

                    foreach (var pp in ab.PowerPlants.Pomps)
                    {
                             UnitedApartmentBuildingsCollection[0].PowerPlants.Pomps.Add(pp);
                    }

                    // Получаем общее число квартир
                         UnitedApartmentBuildingsCollection[0].TotalApartments += ab.TotalApartments;
                      }

                else if (ab.ElectrificationLevel==3)
                {
                    if (UnitedApartmentBuildingsCollection[1].PlanNumber == null)
                        UnitedApartmentBuildingsCollection[1].PlanNumber = ab.PlanNumber.ToString();
                    else
                        UnitedApartmentBuildingsCollection[1].PlanNumber = UnitedApartmentBuildingsCollection[1].PlanNumber + ", " + ab.PlanNumber.ToString();

                    // Перенимает наибольее значение єтажей для определения коефициентов спроса
                    if (ab.Levels > UnitedApartmentBuildingsCollection[1].Levels)
                        UnitedApartmentBuildingsCollection[1].Levels = ab.Levels;

                    // присваивает значение категории 
                    UnitedApartmentBuildingsCollection[1].ElectrificationLevel = ab.ElectrificationLevel;

                    // копирует из каждой коллеуции 
                    foreach (var el in ab.PowerPlants.Elevators)
                    {
                        UnitedApartmentBuildingsCollection[1].PowerPlants.Elevators.Add(el);
                    }

                    foreach (var pp in ab.PowerPlants.Pomps)
                    {
                        UnitedApartmentBuildingsCollection[1].PowerPlants.Pomps.Add(pp);
                    }

                    // Получаем общее число квартир
                    UnitedApartmentBuildingsCollection[1].TotalApartments += ab.TotalApartments;

                }

               

            }

            foreach (var uab in UnitedApartmentBuildingsCollection) uab.ExecuteCalculation();

        }
    }
}
