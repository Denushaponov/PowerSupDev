using System;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.DistrictObjects
{
     public class CommercialBuilding:BindableBase
    {
        public byte PlanNumber { get; set; }
        public string TypeOfCommercial { get; set; }
        public double ValueOfCharacteristics { get; set; }
        public string MeasurmentUnit { get; set; }
        public byte ReliabilityCathegory { get; set; }
        public byte ElectrificationLevel { get; set; }

        public CommercialBuilding()
        {
        }

        // Конструктор для ДБН данных
        public CommercialBuilding(string typeOfCommercial, double valueOfCharacteristics, string measurementUnit, byte reliability, byte electrification)
        {
            TypeOfCommercial = typeOfCommercial;
            ValueOfCharacteristics = valueOfCharacteristics;
            MeasurmentUnit = measurementUnit;
            ReliabilityCathegory = reliability;
            ElectrificationLevel = electrification;
        }
    }
}
