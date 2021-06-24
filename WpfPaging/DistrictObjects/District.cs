using DevExpress.Mvvm;
using DistrictSupplySolution.DbnTables;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.DistrictObjects.BuildingObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<AbstractBuilding> AbstractBuildings { get; set; } = new ObservableCollection<AbstractBuilding>();

        // Рассчет нагрузки микроорайона
        public void CalculateDistrictPower() 
        {
            // Создаю коллекцию специальніх обїектов для определения коеффициентов участия в максимуме
            AbstractBuildings.Clear();
     // Создаю объекты и напоолняю их информацией соответсвтуюего жилого здания
            foreach (var cb in Building.CommercialBuildings)
            {
                AbstractBuilding abstractBuilding = new AbstractBuilding();
                AbstractBuildings.Insert(0, abstractBuilding);
                AbstractBuildings[0].Id = cb.Id;
                AbstractBuildings[0].Type = cb.TypeOfCommercial;//
                AbstractBuildings[0].SideNote = cb.TypeSideNote;
                AbstractBuildings[0].ActivePower = cb.ActiveLoad;
                AbstractBuildings[0].ReactivePower = cb.ReactiveLoad;
                AbstractBuildings[0].FullPower = cb.FullLoad;
            }
         // Также добавляю туда обьекты соответствующие жилым зданиям
            AbstractBuildings = GetUnitedApartmentBuildings(3, AbstractBuildings);
            AbstractBuildings = GetUnitedApartmentBuildings(1, AbstractBuildings);
            // Нахожу максимальную нагрузку
            double maxLoad = AbstractBuildings.Max(ab => ab.FullPower);
            // Создаю объект который будет содержать информацию о здании с максимальной нагрузкой
            AbstractBuilding buildingWithMaxLoad = new AbstractBuilding(); 
            // Наполняю его информацией
            foreach (var ab in AbstractBuildings)
            {
                if (ab.FullPower == maxLoad)
                {
                    buildingWithMaxLoad = ab;
                    break;
                }
            }

            Console.WriteLine(buildingWithMaxLoad.FullPower);
            // Определяю нужный ряд в таблице коеффициентов участия в максимуме  
            int row=0;
            // Создаю таблицу
            CoefficientsOfMembershipInMaximum DbnTable = new CoefficientsOfMembershipInMaximum();
            // Сопоставляю значение в Заметке о типе с номером ряда в таблице

            // Если в здании с макс нагрузкой заметка о типе имеет такое же значение как номер строки , окончить цикл

            // Ограничиваю пользователя от невозможного события(потому что  если вдруг такой потребитель поадёт сюда, он нарушит логику процессов
            if (buildingWithMaxLoad.SideNote != "Особливий")
            {

                // Перечисляю случаи когда получен нужный ряд и пора остановиться.

                if (buildingWithMaxLoad.SideNote == "0")
                {
                    row = 0;
                }

                else if (buildingWithMaxLoad.SideNote == "1")
                {
                    row = 1;
                }

                else if (buildingWithMaxLoad.SideNote == "Їдальня" || buildingWithMaxLoad.SideNote == "Ресторани, кафе")
                {
                    row = 2;

                }
                // Школы и ПТУ
                else if (buildingWithMaxLoad.SideNote == "4" || buildingWithMaxLoad.SideNote == "5" || buildingWithMaxLoad.SideNote == "11")
                {
                    row = 3;
                }

                else if (buildingWithMaxLoad.SideNote == "Установи адміністративного управління, фінансові, проектно-конструкторські організації")
                {
                    row = 4;
                }

                else if (buildingWithMaxLoad.SideNote == "7" || buildingWithMaxLoad.SideNote == "8")
                {
                    row = 5;
                }

                else if (buildingWithMaxLoad.SideNote == "9")
                {
                    row = 6;
                }

                else if (buildingWithMaxLoad.SideNote == "10" || buildingWithMaxLoad.SideNote == "13")
                {
                    row = 8;
                }

                else if (buildingWithMaxLoad.SideNote == "Культові споруди")
                {
                    row = 9;
                }

                else if (buildingWithMaxLoad.SideNote == "12")
                {
                    row = 7;
                }

                // Ни одно здание не подходит пока что под определение комунального обслуживания
                else if (buildingWithMaxLoad.SideNote == "14")
                {
                    // Здания комунального обслуживания
                }

                else if (buildingWithMaxLoad.SideNote == "15")
                {
                    row = 9;
                }

                // Становится известен ряд
                Console.WriteLine(row);

                // Создаю условия выбора колонки в соответствии с примечанием
                // Создаю переменную содержащую номер колонки
                int column=0;
                // Перебираю всю коллекцию
                foreach (var e in AbstractBuildings)
                {
                    if (e.SideNote == "Житлові будинки з електроплитами")
                        column = 0;
                   else if (e.SideNote == "Житлові будинки з газовими плитами або на твердому паливі")
                        column = 1;
                    else if (e.SideNote == "Їдальня")
                        column = 2;
                    else if (e.SideNote == "Ресторани, кафе")
                        column = 3;
                    else if (e.SideNote == "4")
                        column = 4;
                    else if (e.SideNote == "5")
                        column = 5;
                    else if (e.SideNote == "Установи адміністративного управління, фінансові, проектно-конструкторські організації")
                        column = 6;
                    else if (e.SideNote == "7")
                    { column = 7; }
                    else if (e.SideNote == "8"
                        || e.SideNote == "9"
                        || e.SideNote == "10"
                        || e.SideNote =="11"
                        || e.SideNote == "12"
                        || e.SideNote == "13"
                        // Для комунальніх потребителей если они будут добавлені в будущем
                        || e.SideNote == "14")
                        { column = Convert.ToInt32(e.SideNote); }
                    else if (e.SideNote == "Культові споруди")
                        column = 15;
                    

                    // Присваиваем значение коффициента участия в имаксимуме
                    e.CoefficientOfMax = DbnTable.CoefficientsOfMaximum[row, column];

                   if (e.SideNote == "Особливий")
                    {
                        e.CoefficientOfMax = 0.77;
                    }
                }

                
            }

               

            

            
        }

        /// <summary>
        /// Добавляет информацию про жилые дома в коллекцию для определения коэффициентов участия в максимуме
        /// </summary>
        /// <param name="ElectrificationLevel"></param>
        /// <param name="ab"></param>
        /// <returns></returns>
        public ObservableCollection<AbstractBuilding> GetUnitedApartmentBuildings(double ElectrificationLevel, ObservableCollection<AbstractBuilding> ab)
        {
            AbstractBuilding abstractBuilding = new AbstractBuilding();
            
            // Категория 1
            foreach (var uab in Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection)
            {
                if (uab.ElectrificationLevel == ElectrificationLevel && ElectrificationLevel ==1)
                {
                    abstractBuilding.Type = "Житлові будинки з електроплитами";
                    abstractBuilding.SideNote = "0";
                    abstractBuilding.Id = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].Id;
                    abstractBuilding.ActivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingActiveLoad;
                    abstractBuilding.ReactivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingReactiveLoad;
                    abstractBuilding.FullPower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingFullLoad;
                }

                else if (uab.ElectrificationLevel == ElectrificationLevel && ElectrificationLevel == 3)
                {
                    abstractBuilding.Type = "Житлові будинки з газовими плитами або на твердому паливі";
                 
                    abstractBuilding.Id = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].Id;
                    abstractBuilding.SideNote = "1";
                    abstractBuilding.ActivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingActiveLoad;
                    abstractBuilding.ReactivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingReactiveLoad;
                    abstractBuilding.FullPower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingFullLoad;
                }
            }
           
            ab.Insert(0, abstractBuilding);
            return ab;
        }

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
            QuartalInnerLightning = Math.Round( Area * 1.2, 2);
            StreetsTotalLightning = 0;
            foreach (var s in Streets)
            {
                StreetsTotalLightning += Math.Round(s.SpecificLoad * s.TotalLength,2);
            }
            DistrictTotalLightning = Math.Round( QuartalInnerLightning + StreetsTotalLightning,2);
        }

    }

}
