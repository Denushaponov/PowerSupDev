﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistrictSupplySolution.DistrictObjects
{
    /// <summary>
    /// Класс для хранения информации  о здании для рассчёта коэфициента участия в максимуме и нагрузки микрорайноа
    /// </summary>
    public class AbstractBuilding:BindableBase
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string SideNote { get; set; }
        public double ActivePower { get; set; }
        public double ReactivePower { get; set; }
        public double FullPower { get; set; }
        public double CoefficientOfMax { get; set; }
       
     

    }
}
