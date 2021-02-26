using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects.BuildingObjects
{
    public class Entrance:BindableBase
    {
      public ObservableCollection<Elevator> ElevatorsPerEntrance { get; set; }
    }
}
