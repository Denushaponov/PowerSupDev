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

    }
}
