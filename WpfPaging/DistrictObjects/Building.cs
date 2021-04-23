using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects.BuildingObjects;
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

        public UnitedApartmentBuildings UnitedApartmentBuildings { get; set; } = new UnitedApartmentBuildings();

        /// <summary>
        /// Метод позвояет получить обьединённые жилые дома
        /// </summary>
        public void UniteApartmentBuildings()
        {
            if (UnitedApartmentBuildings != null)
            UnitedApartmentBuildings.ApartmentBuildings.Clear();
            foreach (var ab in ApartmentBuildings)
                UnitedApartmentBuildings.ApartmentBuildings.Add(ab);
            UnitedApartmentBuildings.GetUnitedApartmentBuildings();
            
        }
    }
}
