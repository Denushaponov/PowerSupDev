using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class ApartmentBuilding:BindableBase
    {
        public byte PlanNumber { get; set; }
        public double Levels { get; set; }
        private double _entrances;
        public double Entrances
        {
            get { return _entrances; }
            set
            {
                if (_entrances != value)
                {
                    _entrances = value;
                    PowerPlants.RefreshElevators(value);
                }
            }
        }

        public double ApartmentsOnSite { get; set; }
        public byte ElectrificationLevel { get; set; }
        public double ApartmentTgFi { get; set; }
        public byte ReliabilityCathegory { get; set; }
        public double PompPower { get; set; }

        public PowerPlants PowerPlants { get; set; } = new PowerPlants();
        
        public ApartmentBuilding()
        {
            PlanNumber = 0;
            Levels = 0;
            Entrances = 0;
            ApartmentsOnSite = 0;
            ElectrificationLevel = 0;
            ApartmentsOnSite = 0;
            ElectrificationLevel = 0;
            ReliabilityCathegory = 0;
      
           
        }

       
        
        



    }

 
}
