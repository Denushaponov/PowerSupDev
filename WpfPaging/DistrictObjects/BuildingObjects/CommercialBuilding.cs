using System;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Text;
using DistrictSupplySolution.DistrictObjects.ServiceClasses;

namespace WpfPaging.DistrictObjects
{
     public class CommercialBuilding:BindableBase
    {
        public Guid Id =  Guid.NewGuid();
        public byte PlanNumber { get; set; }
        private string _typeOfCommercial;
        public string TypeOfCommercial 
        {
            get { return _typeOfCommercial; }
            set
            {
                if (_typeOfCommercial != value)
                {
                    _typeOfCommercial = value;
                    FindAppropriateCommercial();
                }
            }
        }

        // Содержит дополнительную информацию, для уточнения типа потребителя
        public string TypeSideNote { get; set; }

        private double _valueOfCharacteristics;
        public double ValueOfCharacteristics 
        {
            get { return _valueOfCharacteristics; }
            set
            {
                if (_valueOfCharacteristics != value)
                {
                    _valueOfCharacteristics = value;
                    FindAppropriateCommercial();
                }
            }
        }
        public double SpecificActiveLoad { get; set; }
        public string MeasurmentUnit { get; set; }
        public double CosFi { get; set; }
        public double TgFi { get; set; }
        public byte ReliabilityCathegory { get; set; }
        public byte ElectrificationLevel { get; set; }
        public double ActiveLoad { get; set; }
        public double ReactiveLoad { get; set; }
        public double FullLoad { get; set; }

        // База дынных
        public DbnTables.DbnCommercialBuildings DbnCommercialBuildings;
        public Coordinates BuildingCoordinates { get; set; } = new Coordinates();

        public CommercialBuilding()
        {
          
        }

        public void CalculateCommercialBuildings()
        {
            CalculateActiveLoad();
            CalculateReactiveLoad();
            CalculateFullLoad();
        }

        public void CalculateActiveLoad()
        {
            ActiveLoad = Math.Round(ValueOfCharacteristics * SpecificActiveLoad, 2);
        }

        public void CalculateReactiveLoad()
        {
            ReactiveLoad = Math.Round(ActiveLoad * TgFi, 2);
        }

        public void CalculateFullLoad()
        {
           FullLoad = Math.Round(Math.Sqrt(Math.Pow(ActiveLoad, 2) + Math.Pow(ReactiveLoad, 2)), 2);
        }

        public void FindAppropriateCommercial()
        {
            double i = 0;
            DbnCommercialBuildings = new DbnTables.DbnCommercialBuildings();
            foreach (var c in DbnCommercialBuildings.CommercialBuildingsList)
            {
              
                if (c.TypeOfCommercial == TypeOfCommercial && c.ValueOfCharacteristics >= ValueOfCharacteristics && ValueOfCharacteristics>=i)
                    {
                       
                        MeasurmentUnit = c.MeasurmentUnit;
                        SpecificActiveLoad = c.SpecificActiveLoad;
                        CosFi = c.CosFi;
                        TgFi = c.TgFi;
                    if (c.TypeSideNote!=null)
                    TypeSideNote = c.TypeSideNote;
                      
                    
                }
                if (c.TypeOfCommercial == TypeOfCommercial) i = c.ValueOfCharacteristics;
                else
                    i = 0;
            }
            

        }
    }
}
