using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class District : BindableBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public Building Building { get; set; } = new Building();
        /// <summary>
        /// Улицы
        /// </summary>
       private ObservableCollection<Street> _streets = new ObservableCollection<Street>();
        public ObservableCollection<Street> Streets
        {
            get { return _streets; }
            set { _streets = value; }
        }

        // Поле площадь микрорайона
        public double Area { get; set; }
        // Создать коллекцию улиц
        // Рассчет улиц
        // Рассчет полной мощности микрорайона без учета потерь 
        // Рассчет полной мощности микрорайона с учетом потерь
        // Создать коллекцию ТП

        //Внутриквартальное освешение
        public double QuartalInnerLightning { get; set; }
        public double StreetsTotalLightning { get; set; }
        public double DistrictTotalLightning { get; set; }

       public void CalculateApartmentBuildings() 
        {
         foreach (var ab in Building.ApartmentBuildings)
            {
                ab.ExecuteCalculation();
            }
        }

        public void CalculateCommercialBuildings()
        {
            foreach (var cb in Building.CommercialBuildings)
            {
                cb.CalculateCommercialBuildings();
            }
        }

        public void CalculateLightning()
        {
            QuartalInnerLightning = Area * 1.2;
            StreetsTotalLightning = 0;
            foreach (var s in Streets)
            {
                StreetsTotalLightning += s.SpecificLoad * s.TotalLength;
            }
            DistrictTotalLightning = QuartalInnerLightning + StreetsTotalLightning;
        }

    }

}
