using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DistrictSupplySolution.DistrictObjects
{
    public class OptimizationDataBuilding : BindableBase
    {
        public Guid Id { get; set; }
        public int PlanNumber { get; set; }
        public double CableLength { get; set; } = 0;
        public double FullPower { get; set; }
    }
}
