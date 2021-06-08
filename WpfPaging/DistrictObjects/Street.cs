using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.Mvvm;

namespace DistrictSupplySolution.DistrictObjects
{
    // Класс описывающий улицы 
    public class Street:BindableBase
    {
        double _minLoad;
        public double MinLoad
        {
            get { return _minLoad; }
            set
            {
                _minLoad = value; 
            }
        }

        double _maxLoad;
        public double MaxLoad
        {
            get { return _maxLoad; }
            set
            {
                _maxLoad = value;
            }
        }
        // Категория улицы
        string _category;
        public  string Category {
            get { return _category; }
            set {
                CorrectingRangeOfLoads();
                _category = value; 
            }
        }

        // Общая протяжённость
      public double TotalLength { get; set; }

        // Удельная нагрузка
        double _specificLoad;
      public double SpecificLoad
        { 
            get { return _specificLoad; }
            set 
            {
                CorrectingRangeOfLoads();
                _specificLoad = value;
            } 
        }

        public void CorrectingRangeOfLoads()
        {
        if (Category == "A")
            { MinLoad = 80; MaxLoad = 100; }
        else if (Category == "B")
            { MinLoad = 30; MaxLoad = 80; }
        else
            { MinLoad = 7; MaxLoad = 10; }
        }

    }

    
}
