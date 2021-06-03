using DevExpress.Mvvm;
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

        // Поле площадь микрорайона
        public double Area { get; set; }
        // Создать коллекцию улиц
        // Рассчет улиц
        // Рассчет полной мощности микрорайона без учета потерь 
        // Рассчет полной мощности микрорайона с учетом потерь
        // Создать коллекцию ТП

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

    }

}
