﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class ApartmentBuilding : BindableBase
    {
        public byte PlanNumber { get; set; }
        public double Levels { get; set; }
        public double Entrances { get; set; }
        public double ApartmentsOnSite { get; set; }
        public byte ElectrificationLevel { get; set; }
        public double ApartmentTgFi { get; set; }
        public byte ReliabilityCathegory { get; set; }
        public double FirstElevatorPower { get; set; }
        public double SecondElevatorPower { get; set; }
        public double PompPower { get; set; }

        public ObservableCollection<Pomp> Pomps { get; set; }
        public ObservableCollection<Elevator> Elevators { get; set; }


        public double PompsNumber => CountPomps();
       
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

        

        public double CountElevatorsPerEntrance() 
        {
            double i;
            if (FirstElevatorPower != 0 && SecondElevatorPower != 0)
            { i = 2;
            
            }
            else if (FirstElevatorPower != 0 || SecondElevatorPower != 0)
            { i = 1; }
            else i = 0;
            return i;
        }

        public double CountPomps()
        {
            double i;
            if (PompPower != 0)
            { i = 1; }
            else i = 0;
            return i;

        }




    }

 
}