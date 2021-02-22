using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class PowerPlants:BindableBase
    {
        public ObservableCollection<Pomp> Pomps { get; set; } = new ObservableCollection<Pomp>();
        public ObservableCollection<Elevator> Elevators { get; set; } = new ObservableCollection<Elevator>();
    }
}
