using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistrictSupplySolution.DistrictObjects
{
    /// <summary>
    /// Класс содержащий коэффициент участия в максимуме относительно особенного потребителя
    /// </summary>
    public class SpecialCoefficientOfMax:BindableBase
    {
        public string Type { get; set; }
        public double CoefficientOfMax { get; set;}

    }
}
