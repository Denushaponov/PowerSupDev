using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class Building:BindableBase
    {
        public ObservableCollection<ApartmentBuilding> ApartmentBuildings { get; set; } = new ObservableCollection<ApartmentBuilding>();
        public ObservableCollection<CommercialBuilding> CommercialBuildings { get; set; } = new ObservableCollection<CommercialBuilding>();
    }
}
