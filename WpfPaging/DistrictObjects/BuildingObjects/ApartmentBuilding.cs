using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class ApartmentBuilding:BindableBase
    {
        public byte PlanNumber { get; set; }
        public double Levels { get; set; }
        private double _entrances;
        public double Entrances
        {
            get { return _entrances; }
            set
            {
                if (_entrances != value)
                {
                    _entrances = value;
                    PowerPlants.RefreshElevators(value);
                }
            }
        }

        public double ApartmentsOnSite { get; set; }
        private double _electrificationLevel;
        public double ElectrificationLevel
        {
            get { return _electrificationLevel; }
            set
            {
                _electrificationLevel = value;
                if (value == 3)
                {
                    ApartmentTgFi = 0.2;
                }
                else if (value == 1) ApartmentTgFi = 0.29;
                else ApartmentTgFi = 1;
            }
        }

  
        public double ApartmentTgFi {get; set;}

        
        public double ReliabilityCathegory { get; set; }

        public PowerPlants PowerPlants { get; set; } = new PowerPlants();

        // Рассётные параметры

        public double TotalApartments { get; set; }
        public double ApartmentSpecificLoad { get; set; }
        public double BuildingSpecificActiveLoad { get; set; }
        public double BuildingSpecificReactiveLoad { get; set; }

        public double ElevatorsCofficientOfAsk { get; set; }
        public double ElevatorsActiveLoad { get; set; }
 
        public double ElevatorsReactiveLoad { get; set; }
       

        public double PompsCoefficientOfAsk { get; set; }
        // Удельная нагрузка насосов - сумма введннных пользователем
        public double PompsActiveLoad { get; set; }
        public double PompsReactiveLoad { get; set; }
        

        public double PowerPlantsActiveLoad { get; set; }
        public double PowerPlantsReactiveLoad { get; set; }
       

        public double BuildingActiveLoad { get; set; }
        public double BuildingReactiveLoad { get; set; }
        public double BuildingFullLoad { get; set; }

        // База данных
        public DbnTables.DbnApartmentBuildings DbnApartmentBuildings = new DbnTables.DbnApartmentBuildings();

        public ApartmentBuilding()
        {
            PlanNumber = 0;
            Levels = 0;
            Entrances = 0;
            ApartmentsOnSite = 0;
            ElectrificationLevel = 0;
            ApartmentsOnSite = 0;
            ElectrificationLevel = 0;
            ReliabilityCathegory = 0;
        }

        public void ExecuteCalculation()
        {
            CalculateApartments();
            GetSpecificLoad();
            CalculateBuildingSpecificLoad();
            GetElevatorsCoefficientOfAsk();
            CalcElevatorsLoad();
            CalcPomps();
            CalcPowerPlantsLoad();
            CalcBuildingLoads();
        }

        public void CalculateApartments()
        {
            TotalApartments = Levels * Entrances * ApartmentsOnSite;
        }

        public void GetSpecificLoad()
        {
            ApartmentSpecificLoad = DbnApartmentBuildings.GetApartmentSpecificLoad(TotalApartments, ElectrificationLevel, DbnApartmentBuildings.ApartmentSpecificLoads);
        }

        public void CalculateBuildingSpecificLoad()
        {
            BuildingSpecificActiveLoad = Math.Round(TotalApartments * ApartmentSpecificLoad, 2);
            
        }

        public void GetElevatorsCoefficientOfAsk()
        {
            ElevatorsCofficientOfAsk = DbnApartmentBuildings.GetElevatorCoefficientofAsk(PowerPlants.Elevators.Count, Levels, DbnApartmentBuildings.ElevatorsCoefOfAsk);
        }

        public void CalcElevatorsLoad()
        {
            double totalLoad = 0;
            foreach (var e  in PowerPlants.Elevators)
            {
                totalLoad += e.Load;
            }
            ElevatorsActiveLoad = Math.Round( totalLoad * ElevatorsCofficientOfAsk, 2);
            ElevatorsReactiveLoad = ElevatorsActiveLoad * DbnApartmentBuildings.tgFi.Elevators;
        }

        public void CalcPomps()
        {
            // Получаем удельное суммарное мощность  лифтов
            double elevatorsSpecificLoad = 0;
            foreach (var e in PowerPlants.Elevators)
            {
                elevatorsSpecificLoad += e.Load;
            }
            // Получаем удельное суммарную мощность насосов
            double pompsSpecificLoad = 0;
            foreach (var p in PowerPlants.Pomps)
            {
                pompsSpecificLoad += p.Load;
            }
            double pompPercentage = 100*pompsSpecificLoad/(elevatorsSpecificLoad+pompsSpecificLoad);
            PompsCoefficientOfAsk = DbnApartmentBuildings.GetPompsCoefficientofAsk(PowerPlants.Pomps.Count, pompPercentage, DbnApartmentBuildings.PompsCoefOfAsk);
            PompsActiveLoad = Math.Round(PompsCoefficientOfAsk * pompsSpecificLoad, 2);
            PompsReactiveLoad = PompsActiveLoad * DbnApartmentBuildings.tgFi.Pomps;
        }

        public void CalcPowerPlantsLoad()
        {
            PowerPlantsActiveLoad = PompsActiveLoad + ElevatorsActiveLoad;
            PowerPlantsReactiveLoad = PompsReactiveLoad + ElevatorsReactiveLoad;
        }

        public void CalcBuildingLoads()
        {
            BuildingSpecificReactiveLoad = BuildingSpecificActiveLoad * ApartmentTgFi;
            BuildingActiveLoad = Math.Round(BuildingSpecificActiveLoad + 0.9 * PowerPlantsActiveLoad,2);
            BuildingReactiveLoad = Math.Round(BuildingSpecificReactiveLoad + 0.9*PowerPlantsReactiveLoad, 2);
            BuildingFullLoad = Math.Round(Math.Sqrt(Math.Pow(BuildingActiveLoad, 2)+Math.Pow(BuildingReactiveLoad, 2)), 2);
        }



       


       

    }

 
}
