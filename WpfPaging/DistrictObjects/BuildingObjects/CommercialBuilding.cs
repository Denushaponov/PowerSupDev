using System;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.DistrictObjects
{
     public class CommercialBuilding:BindableBase
    {
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

        // База дынных
        public DbnTables.DbnCommercialBuildings DbnCommercialBuildings;

        public CommercialBuilding()
        {

        }

       

        public void CalculateCommercialBuildings()
        {
            
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
                      
                    
                }
                if (c.TypeOfCommercial == TypeOfCommercial) i = c.ValueOfCharacteristics;

            }
            i = 0;

        }
    }
}
