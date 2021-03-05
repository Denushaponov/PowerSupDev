using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class PowerPlants : BindableBase
    {
        public ObservableCollection<Pomp> Pomps { get; set; } = new ObservableCollection<Pomp>();
        private ObservableCollection<Elevator> _elevatorsPerEntrance { get; set; }
        public ObservableCollection<Elevator> ElevatorsPerEntrance {
            get { return _elevatorsPerEntrance; }
            set
            {
                if (_elevatorsPerEntrance != value)
                {
                    _elevatorsPerEntrance = value;
                    // Отслеживаем изменение числа лифтов на подъезд
                    RefreshElevators(value.Count);
                }
               
            }
        }

        public ObservableCollection<Elevator> Elevators { get; set; } = new ObservableCollection<Elevator>();
     
        /// <summary>
        /// Получение информации об изменении общего количества лифтов при добавлении нового лифта.
        /// </summary>
        /// <param name="entrances">Подъезд</param>
        /// <param name="elevator">Лифт</param>
        public void GetElevators(double entrances, Elevator elevator)
        {
            Elevators.Clear();
            if (ElevatorsPerEntrance == null||Elevators== null)
              { 
                ElevatorsPerEntrance = new ObservableCollection<Elevator>();
               }
            ElevatorsPerEntrance.Insert(0, elevator);
            for (double i=0; i < entrances; i++)
            {
                foreach (var e in ElevatorsPerEntrance)
                    Elevators.Add(e);
            }
        }

        /// <summary>
        /// Обновление коллекциии лифтов если есть изменения в числе подъездов или лифтов в подьезде
        /// </summary>
        /// <param name="entrances">Число подъездов</param>
        public void RefreshElevators(double entrances)
        {

            Elevators.Clear();
            if (ElevatorsPerEntrance != null)
            for (double i = 0; i < entrances; i++)
            {
                
                foreach (var e in ElevatorsPerEntrance)
                    Elevators.Add(e);
            }
        }
       
    }

   

}
