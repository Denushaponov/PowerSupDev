using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Mvvm;

namespace DistrictSupplySolution.DistrictObjects
{
    // Класс описывающий улицы 
    public class Street
    {
        // Категория улицы
      public  string Cathegory { get; set; }

        // Общая протяжённость
      public double TotalLength { get; set; }

        // Удельная нагрузка
      public double SpecificLoad { get; set; }


    }
}
