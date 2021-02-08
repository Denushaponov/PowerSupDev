using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class LoadsApartmentBuilding:ApartmentBuilding
    {
        
         public double TotalApartments => ApartmentsOnSite * Entrances * Levels;



        public double GetTgFiApartments()
        {
            double x;
            if (ElectrificationLevel == 1)
            {
                x = 0.29;
            }
            else if (ElectrificationLevel == 3)
            {
                x = 0.2;
            }
            else
            {
                x = 1;
            }
            return x;
        }

    }
}
