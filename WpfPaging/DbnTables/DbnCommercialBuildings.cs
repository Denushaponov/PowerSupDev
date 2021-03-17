using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.DistrictObjects;

namespace WpfPaging.DbnTables
{
    public class DbnCommercialBuildings:BindableBase
    {
        public List<CommercialBuilding> CommercialBuildingsList { get; set;}
        public DbnCommercialBuildings()
        {
            // Вносить значения из дбн
            CommercialBuildingsList.Add(new CommercialBuilding());
        }
    }
}
