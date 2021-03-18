using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class District : BindableBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public Building Building { get; set; } = new Building();

       public void CalculateApartmentBuildings() 
        {
         foreach (var ab in Building.ApartmentBuildings)
            {
                ab.ExecuteCalculation();
            }
        }

        public void CalculateCommercialBuildings()
        {
            foreach (var cb in Building.CommercialBuildings)
            {
                cb.CalculateCommercialBuildings();
            }
        }

    }

}
