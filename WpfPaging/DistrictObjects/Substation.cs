using DevExpress.Mvvm;
using DistrictSupplySolution.DbnTables;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.DistrictObjects.BuildingObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
namespace DistrictSupplySolution.DistrictObjects
{
    public class Substation:BindableBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
      //  public AbstractBuilding 
         public string Name { get; set; }
    }
}
