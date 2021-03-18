using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.DistrictObjects;

namespace WpfPaging.DbnTables
{
    public class DbnCommercialBuildings:BindableBase
    {
        public List<DbnCommercialBuilding> CommercialBuildingsList { get; set; }
        public DbnCommercialBuildings()
        {
            // Вносить значения из дбн
            CommercialBuildingsList = new List<DbnCommercialBuilding>()
            {
            new DbnCommercialBuilding("Підприємства громадського харчування повністю електрифіковані", 500,  1.03, "кВт на місце", 0.98, 0.2),
            new DbnCommercialBuilding("Підприємства громадського харчування повністю електрифіковані", 1000, 0.85, "кВт на місце", 0.98, 0.2),
            new DbnCommercialBuilding("Підприємства громадського харчування повністю електрифіковані", 50000, 0.75, "кВт на місце", 0.98, 0.2),
            new DbnCommercialBuilding("Підприємства громадського харчування частково електрифіковані", 500, 0.8, "кВт на місце", 0.95, 0.33),
            new DbnCommercialBuilding("Підприємства громадського харчування частково електрифіковані", 1000, 0.7, "кВт на місце", 0.95, 0.33),
            new DbnCommercialBuilding("Підприємства громадського харчування частково електрифіковані", 50000, 0.6, "кВт на місце", 0.95, 0.33),
            };
            
        }

        public class DbnCommercialBuilding
        {
            public string TypeOfCommercial { get; set; }
            public double ValueOfCharacteristics { get; set; }
            public double SpecificActiveLoad { get; set; }
            public string MeasurmentUnit { get; set; }
            public double CosFi { get; set; }
            public double TgFi { get; set; }

            public DbnCommercialBuilding(string type, double valueOf, double load, string measurmentUnit, double cosFi, double tgFi)
            {
                TypeOfCommercial = type;
                ValueOfCharacteristics = valueOf;
                SpecificActiveLoad = load;
                MeasurmentUnit = measurmentUnit;
                CosFi = cosFi;
                TgFi = tgFi;
            }
        }

        
    }
}
