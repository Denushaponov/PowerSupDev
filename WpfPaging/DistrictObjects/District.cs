using DevExpress.Mvvm;
using DistrictSupplySolution;
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
        public AllBuildings Building { get; set; } = new AllBuildings();
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

        double _quartalInnnerLightning;

        public double QuartalInnerLightning
        {
            get
            {
                return _quartalInnnerLightning;
            }
            set
            {
                _quartalInnnerLightning = value;
                // Проверка действительно ли программа провёла рассчёт  освещения
                if (_quartalInnnerLightning != 0)
                {
                    IsLightningCalculated = true;
                }
                else
                {
                    IsLightningCalculated = false;
                }
            }
        }
        public double StreetsTotalLightning { get; set; }
        public double DistrictTotalLightning { get; set; }
        public ObservableCollection<AbstractBuilding> AbstractBuildings { get; set; } = new ObservableCollection<AbstractBuilding>();

        // Рассчет нагрузки микроорайона
        public bool IsLightningCalculated { get; set; } = false;
        public bool IsReadyForCalculation { get; set; } = false;


        // Нагрузка микрорайона c учётом потерь
        public double ActivePowerOfDistrict { get; set; }
        public double ReactivePowerOfDistrict { get; set; }
        public double FullPowerOfDistrict { get; set; }


        /// <summary>
        /// Готов ли микрорайон к определению коєффициентов участия в максимуме
        /// </summary>
        public bool IsReadyToDetermineCOP { get; set; } = false;

        /// <summary>
        /// Список вариантов микрорайона которые будут оптимизироваться
        /// </summary>
        public List<District> OptiDistricts { get; set; }
        public ObservableCollection<OptimizationDataBuilding> OptimizationDataBuildings { get; set; } = new ObservableCollection<OptimizationDataBuilding>();

        /// <summary>
        /// List of all substations and is generated automatically
        /// </summary>
        public ObservableCollection<Substation> Substations { get; set; }
        /// <summary>
        /// Номинальная нагрузка  трансформатора
        /// </summary>
        public int TransformerLoad { get; set; }
        /// <summary>
        /// Число Трансформаторов на подстанции
        /// </summary>
        public int NumberOfTransformers { get; set; }
        /// <summary>
        /// Коеффициент загрузки ТП
        /// </summary>
        public double CoeffOfLoadSubstation { get; set; }

        #region Optimization parameters
        public double MinCoeffOfLoadSubstation { get; set; }
        public double MaxCoeffOfLoadSubstation { get; set; }
        public double MaxCalbeLength { get; set; }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CoefOfLoad">Юзер коефициент нагрузки</param>
        /// <returns>Formed substation list</returns>
        public ObservableCollection<Substation> DetermineSubstationsList()
        {
            OptimizationDataBuildings = new ObservableCollection<OptimizationDataBuilding>();
            ConvertBuildingsToOptiDataBuildings();
            ObservableCollection<Substation> SB = new ObservableCollection<Substation>();
            string tp = "ТП";
            // Результат = количество подстанций
            if (TransformerLoad * CoeffOfLoadSubstation * NumberOfTransformers != 0)
            {
                int _ = Convert.ToInt32(Math.Ceiling(FullPowerOfDistrict / (CoeffOfLoadSubstation * TransformerLoad * NumberOfTransformers)));
                for (int i = 0; i < _; i++)
                {
                    Substation substation = new Substation();
                    substation.Id = Guid.NewGuid();
                    substation.Name = tp + Convert.ToString(i + 1);
                    substation.IsLengthsCompleted = false;
                    substation.OptimizationDataBuildings = OptimizationDataBuildings;
                    SB.Add(substation);
                }
                return SB;
            }
            else return default;
        }


        public void ConvertBuildingsToAbstractBuildings()
        {
            // Создаю коллекцию специальніх обїектов для определения коеффициентов участия в максимуме

            // Создаю объекты и напоолняю их информацией соответсвтуюего жилого здания

            foreach (var cb in Building.CommercialBuildings)
            {
                if (ValidationRuleAbstractBuildings(AbstractBuildings, cb.PlanNumber.ToString(), cb.FullLoad))
                {
                    AbstractBuilding abstractBuilding = new AbstractBuilding();
                    AbstractBuildings.Insert(0, abstractBuilding);
                    AbstractBuildings[0].Id = cb.Id;
                    AbstractBuildings[0].Type = cb.TypeOfCommercial;//
                    AbstractBuildings[0].SideNote = cb.TypeSideNote;
                    AbstractBuildings[0].ActivePower = cb.ActiveLoad;
                    AbstractBuildings[0].ReactivePower = cb.ReactiveLoad;
                    AbstractBuildings[0].FullPower = cb.FullLoad;
                    AbstractBuildings[0].PlanNumber = cb.PlanNumber.ToString();
                }
            }
            // Также добавляю туда обьекты соответствующие жилым зданиям
            AbstractBuildings = GetUnitedApartmentBuildings(3, AbstractBuildings);
            AbstractBuildings = GetUnitedApartmentBuildings(1, AbstractBuildings);
        }


        public void ConvertBuildingsToOptiDataBuildings()
        {
            // Создаю коллекцию специальніх обїектов для определения коеффициентов участия в максимуме

            // Создаю объекты и напоолняю их информацией соответсвтуюего жилого здания

            foreach (var cb in Building.CommercialBuildings)
            {
                if (ValidationRuleOptiData(OptimizationDataBuildings, cb.PlanNumber, cb.FullLoad))
                {
                    if (cb.PlanNumber !=0)
                    {
                    OptimizationDataBuilding odb = new OptimizationDataBuilding();
                    OptimizationDataBuildings.Insert(0, odb);
                    OptimizationDataBuildings[0].Id = cb.Id;
                    OptimizationDataBuildings[0].PlanNumber = cb.PlanNumber;
                    OptimizationDataBuildings[0].FullPower = cb.FullLoad;
                    }
                }
            }
            // Также добавляю туда обьекты соответствующие жилым зданиям
            foreach (var ab in Building.ApartmentBuildings)
            {
                if (ValidationRuleOptiData(OptimizationDataBuildings, ab.PlanNumber, ab.BuildingFullLoad))
                {
                    if (ab.PlanNumber != 0)
                    {
                        OptimizationDataBuilding odb = new OptimizationDataBuilding();
                        OptimizationDataBuildings.Insert(0, odb);
                        OptimizationDataBuildings[0].Id = ab.Id;
                        OptimizationDataBuildings[0].PlanNumber = ab.PlanNumber;
                        OptimizationDataBuildings[0].FullPower = ab.BuildingFullLoad;
                    }
                }
            }
        }



        /// <summary>
        /// Определение коэффициентов участия в максимуме
        /// </summary>
        public void DetermineCoefficientsOfParticipanceInMaximumLoad()
        {
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
            int row = 0;
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
                int column = 0;
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
                    else if (e.SideNote == "Установи адміністративного управління, фінансові, проектно-конструкторські організації")
                        column = 6;
                    else if (
                           e.SideNote == "4"
                        || e.SideNote == "5"
                        || e.SideNote == "7"
                        || e.SideNote == "8"
                        || e.SideNote == "9"
                        || e.SideNote == "10"
                        || e.SideNote == "11"
                        || e.SideNote == "12"
                        || e.SideNote == "13"
                        // Для комунальніх потребителей если они будут добавлені в будущем
                        || e.SideNote == "14")
                    { column = Convert.ToInt32(e.SideNote); }
                    else if (e.SideNote == "Культові споруди")
                        column = 15;

                    // Присваиваем значение коффициента участия в максимуме
                    e.CoefficientOfMax = DbnTable.CoefficientsOfMaximum[row, column];

                    // Выношу в отдельную строку так как здесь нету способа определить номер колонки.
                    if (e.SideNote == "Особливий визначений")
                    {
                        // поведение для особенных потребителей с определённым пользователем набором коэффициентов
                        e.CoefficientOfMax = e.SpecialConsumerCoefficientsOfMax[row].CoefficientOfMax;
                    }

                    // То здание, которое с максимальной нагрузкой обладает коэфициентом 1, выставляю
                    if (e.Id == buildingWithMaxLoad.Id)
                    {
                        e.CoefficientOfMax = 1;
                    }


                    else if (e.Type == "Освітлення мікрорайону")
                    { e.CoefficientOfMax = 1; }


                }
            }
        }

        /// <summary>
        /// Добавляет информацию про жилые дома в коллекцию для определения коэффициентов участия в максимуме
        /// </summary>
        /// <param name="ElectrificationLevel">Уровень Электрификации</param>
        /// <param name="ab">Обновляемая коллекция</param>
        /// <returns></returns>
        public ObservableCollection<AbstractBuilding> GetUnitedApartmentBuildings(double ElectrificationLevel, ObservableCollection<AbstractBuilding> ab)
        {
            AbstractBuilding abstractBuilding = new AbstractBuilding();

            foreach (var uab in Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection)
            {


                if (uab.ElectrificationLevel == ElectrificationLevel && ElectrificationLevel == 1)
                {
                    abstractBuilding.Type = "Житлові будинки з електроплитами";
                    abstractBuilding.SideNote = "0";
                    abstractBuilding.Id = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].Id;
                    abstractBuilding.ActivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingActiveLoad;
                    abstractBuilding.ReactivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingReactiveLoad;
                    abstractBuilding.FullPower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].BuildingFullLoad;
                    abstractBuilding.PlanNumber = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[0].PlanNumber;
                }

                else if (uab.ElectrificationLevel == ElectrificationLevel && ElectrificationLevel == 3)
                {
                    abstractBuilding.Type = "Житлові будинки з газовими плитами або на твердому паливі";

                    abstractBuilding.Id = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].Id;
                    abstractBuilding.SideNote = "1";
                    abstractBuilding.ActivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingActiveLoad;
                    abstractBuilding.ReactivePower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingReactiveLoad;
                    abstractBuilding.FullPower = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].BuildingFullLoad;
                    abstractBuilding.PlanNumber = Building.UnitedApartmentBuildings.UnitedApartmentBuildingsCollection[1].PlanNumber;
                }
            }

            if (ValidationRuleAbstractBuildings(AbstractBuildings, abstractBuilding.PlanNumber, abstractBuilding.FullPower) == true)
                ab.Insert(0, abstractBuilding);
            return ab;
        }

        public bool ValidationRuleAbstractBuildings(ObservableCollection<AbstractBuilding> abstractBuildings, string PlanNumber, double FullPower)
        {

            bool abNeedsUpdate = false;


            // Проверяю наличие зданий с параметрами переданными функции в коллекции abstractBuildings
            int trackerOfExistingAb = abstractBuildings.Where(abs => abs.PlanNumber == PlanNumber && abs.FullPower == FullPower).Count();
            // Проверяю наличие зданий с параметром PlanNumber переданными функции  и отличным 
            // от переданного функции FullPower в коллекции abstractBuildings
            int trackerOfChangedAb = abstractBuildings.Where(abs => abs.PlanNumber == PlanNumber && abs.FullPower != FullPower).Count();
            // Если параметры функции и параметры здания совпадают, здание добавлено в коллекцию
            // Тогда не требуется обновление данного здания и я его оставляю
            if (trackerOfExistingAb >= 1)
            { abNeedsUpdate = false; return abNeedsUpdate; }

            // Если PlanNumber здания и параметра функции совпадают тогда 
            // Здание добавляется в коллекцию изменённых
            // В случае когда полученное здние является измнненым
            if (trackerOfChangedAb >= 1)
            {
                //  нужно обновлять соответствующий обьект
                abNeedsUpdate = true;
                // Добавляю счётчик порядкового номера
                int i = 0;
                AbstractBuilding abstractBuildingToRemove = new AbstractBuilding();
                // Перебираю коллекцию АбстрактБилдинг
                foreach (var a in AbstractBuildings)
                {
                    // Сравниваю каждый абстракт билдинг с параметром функции 
                    // Если они не совпадают 
                    if (a.PlanNumber != PlanNumber)
                    {
                        // Тогда счетчик добавляет единицу
                        i++;
                    }
                    // Совпадают
                    else
                    {
                        // Значит текущее здание нужно запомнить как подлежащее удалению
                        abstractBuildingToRemove = a;
                        break;
                    }
                }
                // Если подлежаще удалению здание было определено, то удаляем его из кооллекции
                if (abstractBuildingToRemove != null) AbstractBuildings.Remove(abstractBuildingToRemove);
            }
            // Если здание не существует в коллекции AbstractBuildings и не было изменено 
            if (trackerOfExistingAb == 0 && trackerOfChangedAb == 0)
            {
                // То следует его добавить в коллекцию
                abNeedsUpdate = true;
            }

            return abNeedsUpdate;
        }


        public bool ValidationRuleOptiData(ObservableCollection<OptimizationDataBuilding> optimizationDataBuildings, int PlanNumber, double FullPower)
        {

            bool abNeedsUpdate = false;

            if (optimizationDataBuildings != null)
            {

                // Проверяю наличие зданий с параметрами переданными функции в коллекции abstractBuildings
                int trackerOfExistingAb = optimizationDataBuildings.Where(abs => abs.PlanNumber == PlanNumber && abs.FullPower == FullPower).Count();
                // Проверяю наличие зданий с параметром PlanNumber переданными функции  и отличным 
                // от переданного функции FullPower в коллекции abstractBuildings
                int trackerOfChangedAb = optimizationDataBuildings.Where(abs => abs.PlanNumber == PlanNumber && abs.FullPower != FullPower).Count();
                // Если параметры функции и параметры здания совпадают, здание добавлено в коллекцию
                // Тогда не требуется обновление данного здания и я его оставляю
                if (trackerOfExistingAb >= 1)
                { abNeedsUpdate = false; return abNeedsUpdate; }

                // Если PlanNumber здания и параметра функции совпадают тогда 
                // Здание добавляется в коллекцию изменённых
                // В случае когда полученное здние является измнненым
                if (trackerOfChangedAb >= 1)
                {
                    //  нужно обновлять соответствующий обьект
                    abNeedsUpdate = true;
                    // Добавляю счётчик порядкового номера
                    int i = 0;
                    OptimizationDataBuilding optiDataBuildingToRemove = new OptimizationDataBuilding();
                    // Перебираю коллекцию АбстрактБилдинг
                    foreach (var a in OptimizationDataBuildings)
                    {
                        // Сравниваю каждый абстракт билдинг с параметром функции 
                        // Если они не совпадают 
                        if (a.PlanNumber != PlanNumber)
                        {
                            // Тогда счетчик добавляет единицу
                            i++;
                        }
                        // Совпадают
                        else
                        {
                            // Значит текущее здание нужно запомнить как подлежащее удалению
                            optiDataBuildingToRemove = a;
                            break;
                        }
                    }
                    // Если подлежаще удалению здание было определено, то удаляем его из кооллекции
                    if (optiDataBuildingToRemove != null) OptimizationDataBuildings.Remove(optiDataBuildingToRemove);
                }
                // Если здание не существует в коллекции AbstractBuildings и не было изменено 
                if (trackerOfExistingAb == 0 && trackerOfChangedAb == 0)
                {
                    // То следует его добавить в коллекцию
                    abNeedsUpdate = true;
                }
            }
            else abNeedsUpdate = true;

            return abNeedsUpdate;
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
            QuartalInnerLightning = Math.Round(Area * 1.2, 2);
            StreetsTotalLightning = 0;
            foreach (var s in Streets)
            {
                StreetsTotalLightning += Math.Round(s.SpecificLoad * s.TotalLength, 2);
            }
            DistrictTotalLightning = Math.Round(QuartalInnerLightning + StreetsTotalLightning, 2);
            ConvertTotalLigtningToAbstractBuildings();
        }

        /// <summary>
        /// конверитрую освещение в формат для рассчёта коефициента участия в максимуме
        /// </summary>
        public void ConvertTotalLigtningToAbstractBuildings()
        {

            if (ValidationRuleAbstractBuildings(AbstractBuildings, "0", DistrictTotalLightning))
            {

                AbstractBuilding ab = new AbstractBuilding();
                ab.ActivePower = DistrictTotalLightning;
                ab.FullPower = ab.ActivePower;
                ab.Type = "Освітлення мікрорайону";
                ab.PlanNumber = "0";
                AbstractBuildings.Insert(0, ab);
            }
        }


        public void CalculateDistrictPower()
        {
            double PreliminaryActivePowerOfDistrict = 0;
            double PreliminaryReactivePowerOfDistrict = 0;
            double PreliminaryFullPowerOfDistrict = 0;

            foreach (var ab in AbstractBuildings)
            {
                PreliminaryActivePowerOfDistrict += Math.Round(ab.ActivePower, 2);
                PreliminaryReactivePowerOfDistrict += Math.Round(ab.ReactivePower, 2);
                PreliminaryFullPowerOfDistrict = Math.Round(Math.Sqrt(Math.Pow(PreliminaryActivePowerOfDistrict, 2) + Math.Pow(PreliminaryReactivePowerOfDistrict, 2)), 2);
            }
            double ActivePowerLossesInVT = 0.02 * PreliminaryFullPowerOfDistrict;
            double ReactivePowerLossesInVT = 0.01 * PreliminaryFullPowerOfDistrict;
            double ActivePowerLossesInLines = 0.03 * PreliminaryFullPowerOfDistrict;

            ActivePowerOfDistrict = PreliminaryActivePowerOfDistrict + ActivePowerLossesInVT + ActivePowerLossesInLines;
            ReactivePowerOfDistrict = ReactivePowerLossesInVT + PreliminaryReactivePowerOfDistrict;
            FullPowerOfDistrict = Math.Round(Math.Sqrt(Math.Pow(ActivePowerOfDistrict, 2) + Math.Pow(ReactivePowerOfDistrict, 2)), 2);
        }

        public void OptiProc()
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now + " ");
            int i = 0;
            //тут можно рассчитать каждый микрорайон
            List<List<List<int>>> combinationsList = new List<List<List<int>>>();
            foreach (var ts in Substations)
            {
                List<List<int>> hyy = ts.DefineCombinationsPerSubstation(NumberOfTransformers, TransformerLoad, MinCoeffOfLoadSubstation, MaxCoeffOfLoadSubstation, MaxCalbeLength);
                combinationsList.Insert(i,hyy);
                i++;
            }
            System.Diagnostics.Debug.WriteLine(DateTime.Now + " CPS");
            List<List<List<int>>> result = new List<List<List<int>>>();
            result = CPS(combinationsList);
            combinationsList.Clear();
            var x = CalculateDistricts(result);

            //result = WithoutintersectionsProduct(result.ToList());
            ///11111111111
          //  System.Diagnostics.Debug.WriteLine(DateTime.Now + " Listing");
          //  foreach (var e in result)
          //  {
          //      List<int> comparer = new List<int>();
          //      foreach (var u in e)
          //      {
          //          comparer.AddRange(u);  
          //      }
                
          //      if (comparer.GroupBy(x => x).All(g => g.Count() == 1))
          //      {
          //           List<List<int>> x = e.ToList();
          //          combinationsList2.Insert(0, x);
          //      }
          //  }
          //  System.Diagnostics.Debug.WriteLine(DateTime.Now + " Intersection");
          // // combinationsList2 = WithoutintersectionsProduct(combinationsList2);
            
            
          // ///1111111111111
          //  System.Diagnostics.Debug.WriteLine(combinationsList2.Count());
          //  System.Diagnostics.Debug.WriteLine(DateTime.Now + " Intersection");
          ////  var JK = result.Inrersec();
          // // System.Diagnostics.Debug.WriteLine(JK.Count());
          //  Console.WriteLine(DateTime.Now);

          //  Console.WriteLine(result.Count());
          //  Console.ReadKey();
        }

        public District()
        {
            Streets = new ObservableCollection<Street> { new Street { Category = "A" }, new Street { Category = "B" }, new Street { Category = "C" }, new Street { Category = "D" } };
        }
        // Create funtion of auto calculate district
        public District Auto()
        {
            District CalculatedDistrict = new District();
            CalculatedDistrict.Building.ApartmentBuildings = Building.ApartmentBuildings;
            CalculatedDistrict.Building.CommercialBuildings = Building.CommercialBuildings;
            CalculatedDistrict.Building.UniteApartmentBuildings();
            CalculatedDistrict.ConvertBuildingsToAbstractBuildings();
            foreach (var ab in AbstractBuildings)
            {
                foreach (var abs in CalculatedDistrict.AbstractBuildings)
                {
                    if (abs.FullPower == ab.FullPower && abs.PlanNumber == ab.PlanNumber && ab.SpecialConsumerCoefficientsOfMax != default)
                    {
                        abs.SpecialConsumerCoefficientsOfMax = ab.SpecialConsumerCoefficientsOfMax;
                        abs.SideNote = ab.SideNote;
                    }
                }
            }
            CalculatedDistrict.DetermineCoefficientsOfParticipanceInMaximumLoad();
            //Разобраться с логикой для освещения
            CalculatedDistrict.DistrictTotalLightning = DistrictTotalLightning;
            CalculatedDistrict.ConvertTotalLigtningToAbstractBuildings();

            CalculatedDistrict.CalculateDistrictPower();
            return CalculatedDistrict;
        }

        // Create funtion of auto calculate district modified
        public District Auto2(District source, List<int> PlanNumbers)
        {
            District CalculatedDistrict = new District();
           // CalculatedDistrict.Building.ApartmentBuildings = Building.ApartmentBuildings;
            foreach (var p in PlanNumbers)
            {
                foreach (var ab in source.Building.ApartmentBuildings)
                { if (ab.PlanNumber == p) CalculatedDistrict.Building.ApartmentBuildings.Add(ab); }
                foreach (var cb in source.Building.CommercialBuildings)
                { if (cb.PlanNumber == p) CalculatedDistrict.Building.CommercialBuildings.Add(cb); }
            }
          // CalculatedDistrict.Building.CommercialBuildings = Building.CommercialBuildings;
            CalculatedDistrict.Building.UniteApartmentBuildings();
            CalculatedDistrict.ConvertBuildingsToAbstractBuildings();
            foreach (var ab in source.AbstractBuildings)
            {
                foreach (var abs in CalculatedDistrict.AbstractBuildings)
                {
                    // Если абс ьланк то удалить абс ТОДО
                    if (abs.FullPower == ab.FullPower && abs.PlanNumber == ab.PlanNumber && ab.SpecialConsumerCoefficientsOfMax != default)
                    {
                        abs.SpecialConsumerCoefficientsOfMax = ab.SpecialConsumerCoefficientsOfMax;
                        abs.SideNote = ab.SideNote;
                    }
                }
            }
            CalculatedDistrict.DetermineCoefficientsOfParticipanceInMaximumLoad();
            //Разобраться с логикой для освещения
          //  CalculatedDistrict.DistrictTotalLightning = DistrictTotalLightning;
           // CalculatedDistrict.ConvertTotalLigtningToAbstractBuildings();
            CalculatedDistrict.CalculateDistrictPower();
            return CalculatedDistrict;
        }

        public List<List<District>> CalculateDistricts(List<List<List<int>>> set)
        {
            List<List<District>> result = new List<List<District>>();
            foreach (var district in set)
            {
                int remainingSubstations = Substations.Count();
                double tLightning = DistrictTotalLightning;
                double remainingTLightning = tLightning;

                double maximumSubsLoad = TransformerLoad * 2 * MaxCoeffOfLoadSubstation;
                double minSubsLoad = TransformerLoad*2 * MinCoeffOfLoadSubstation;
        
                List<District> Subs = new List<District>();
                List<int> i=new List<int>();
                foreach (var substation in district)
                {
                    District calc = Auto2(this, substation);
                    if (calc.FullPowerOfDistrict < maximumSubsLoad && calc.FullPowerOfDistrict + (tLightning / remainingSubstations) * 3 > minSubsLoad)
                    {
                        double possibleLoad = maximumSubsLoad - calc.FullPowerOfDistrict;

                        if (possibleLoad >= 0)
                        {
                            Subs.Add(calc);
                        }
                        
                    }
                }

                // Распределение освещения
                if (Subs.Count() == Substations.Count() /*&& remainingTLightning == 0*/ && Subs.All(o => o.FullPowerOfDistrict <= maximumSubsLoad && o.FullPowerOfDistrict+DistrictTotalLightning*0.9 >= minSubsLoad))
                {
                    double tempLightningDist = DistrictTotalLightning;//осветительная нагрузка
                    var x = from s in Subs select s.Id; // Старый порядок подстанций
                    Subs = Subs.OrderBy(o => o.FullPowerOfDistrict).ToList();
                    foreach (var s in Subs)
                    {
                        double toAdd = maximumSubsLoad - s.FullPowerOfDistrict;
                        if (tempLightningDist >= toAdd)
                        {
                            s.FullPowerOfDistrict = Math.Round(s.FullPowerOfDistrict + toAdd, 2);
                            tempLightningDist -= toAdd;
                        }
                        else // tempLightningDist < toAdd
                        {
                            s.FullPowerOfDistrict = Math.Round(s.FullPowerOfDistrict + tempLightningDist, 2);
                            tempLightningDist -= tempLightningDist;
                        }
                    }
                    List<District> res = new List<District>();
                    foreach (var id in x)
                    {
                        foreach (var s in Subs)
                        { if (s.Id == id) res.Add(s); }
                    }

                    result.Add(res); 
                }
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Count Result: " + result.Count());
            }    
            return result;
        }

        //!!!!!!!!!!!!!!!!!!!1 FOR TEST
        public List<List<List<int>>> CPS(List<List<List<int>>> sequences)
        {
            int numberOfBuildings = Substations[0].OptimizationDataBuildings.Count();
            int i = 0;
            List<List<List<int>>> result = new List<List<List<int>>>();
            foreach (var sequence in sequences)
            {
                // List<List<int>> localSequence = sequence.OrderBy(a => a.Sort(b => b)).Distinct().ToList();
                List<List<int>> localSequence = new List<List<int>>(); //sequence.OrderByDescending(a => a.(b => b.)).Distinct().ToList();
                System.Diagnostics.Debug.WriteLine("Number of elements: " + sequence.Count() + " before removing duplicates");
                foreach (var seq in sequence)
                {
                    List<int> seqOrdered = seq.OrderByDescending(s => s).ToList();
                    localSequence.Add(seqOrdered);
                }
                    localSequence = localSequence.Distinct().ToList();
                System.Diagnostics.Debug.WriteLine("Number of elements: " + localSequence.Count() + " after removing duplicates");

                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Iteration #" + i + " Start");
                if (i == 0)
                {
                    foreach (var x in localSequence)
                    {
                        List<List<int>> tempRes = new List<List<int>>();
                       
                        tempRes.Add(x.OrderBy(s => s).ToList());
                        result.Add(tempRes.Distinct().ToList());
                    }
                }
                else
                {
                    List<List<List<int>>> tempResult = new List<List<List<int>>>();
                    //tempResult = result;
                    // result = new List<List<List<int>>>();

                    foreach (var prev in result)
                    {
                        foreach (var curr in localSequence)
                        {

                            List<List<int>> temp = new List<List<int>>();
                            temp.AddRange(prev);
                            List<int> orderedCurr = curr.OrderBy(s => s).ToList();
                            temp.Add(orderedCurr);
                            List<int> comparer = temp.SelectMany(x => x).ToList();
                            if (comparer.GroupBy(x => x).All(g => g.Count() == 1))
                                tempResult.Add(temp);
                        }
                    }
                    result = new List<List<List<int>>>(tempResult.Distinct());
                    result = tempResult;
                }
                System.Diagnostics.Debug.WriteLine(DateTime.Now + " Iteration #" + i + " END");
                System.Diagnostics.Debug.WriteLine("Number of elements: " + result.Count());
                i++;
            }

            List<List<List<int>>> difference = new List<List<List<int>>>();
            List<int> plannumGen = new List<int>();//test
            foreach (var res in result)
            {
                List<int> test = new List<int>();
                List<int> plannum = new List<int>();//test
                foreach (var ob in OptimizationDataBuildings)
                {
                    plannum.Add(ob.PlanNumber);
                }

                foreach (var o in res)
                {
                    test.AddRange(o);
                }
                if (test.Count == numberOfBuildings) //тест
                    difference.Add(res.ToList());
                var x = plannum.Except(test);//test
                plannumGen.AddRange(x.ToList()); //test
            }
            plannumGen = plannumGen.Distinct().ToList(); //test
          //  result = new List<List<List<int>>>(difference);
            return difference;
        }

    }

}
