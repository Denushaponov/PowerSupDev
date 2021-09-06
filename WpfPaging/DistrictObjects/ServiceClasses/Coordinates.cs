using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistrictSupplySolution.DistrictObjects.ServiceClasses
{
    public class Coordinates:BindableBase
    {
        /// <summary>
        /// Ось Х
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Ось Y
        /// </summary>
        public double Y { get; set; }
    }
}
