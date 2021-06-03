using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects.BuildingObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class Building : BindableBase
    {
        ObservableCollection<ApartmentBuilding> _apartmentBuildings = new ObservableCollection<ApartmentBuilding>();
        public ObservableCollection<ApartmentBuilding> ApartmentBuildings
        {
            get { return _apartmentBuildings; }
            set
            {

                _apartmentBuildings = value;

            }
        }
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

        public bool Validate(Guid id, byte planNumber)
        {

            bool IsDuplicate;
              
                List<byte> planNumbers = new List<byte>();
            foreach (var ab in ApartmentBuildings)
            {
                
                planNumbers.Add(ab.PlanNumber);
            }
            foreach (var cb in CommercialBuildings)
            {
               
                planNumbers.Add(cb.PlanNumber);
            }

            int x = (from p in planNumbers where p == planNumber select p).Count();
            if (x > 1)
            { IsDuplicate = true; }
            else IsDuplicate = false;
            return IsDuplicate;
          
            
          

            


        }



        // public void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //
        //     foreach (var e in _apartmentBuildings)
        //     {
        //         e.PropertyChanged += EntityPropertyChanged;
        //     }
        //
        //     Console.WriteLine("!!!");
        //     
        //     //This will get called when the property of an object inside the collection changes
        // }
    }  

}
