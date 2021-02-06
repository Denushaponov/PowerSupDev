using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class LoadsApartmentBuilding:ApartmentBuilding
    {
        
       // public double TgFiApartments => GetElectrification();
       // public double TotalApartments => ApartmentsOnSite * Entrances;


        public double GetElectrification()
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
