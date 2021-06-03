using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;
using WpfPaging.DistrictObjects;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using OfficeOpenXml;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WpfPaging.ViewModels
{
    public class ApartmentsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;
       

        /// <summary>
        /// Выбранный микрорайон (в DistrictMenuVM), присылается для изменений
        /// </summary>
        private District _selectedDistrict = new District();
        public District SelectedDistrict
        {
            get { return _selectedDistrict; }
            set
            {

                _selectedDistrict = value;
                foreach (var e in _selectedDistrict.Building.ApartmentBuildings)
                {
                    e.PropertyChanged += ValidationRuleApartmentBuildings;
                }



            }
        }

        private ApartmentBuilding _selectedApartmentBuilding = new ApartmentBuilding();
        public ApartmentBuilding SelectedApartmentBuilding
        {
            get { return _selectedApartmentBuilding; }
            set
            {
               
                _selectedApartmentBuilding = value;
                foreach (var e in _selectedDistrict.Building.ApartmentBuildings)
                {
                    e.PropertyChanged += ValidationRuleApartmentBuildings;
                }


            }
        }


        public Elevator SelectedElevator { get; set; }





        /// <summary>
        /// Команды и параметры для взаимодействия между окнами
        /// </summary>
        /// <param name="pageService"></param>
        /// <param name="eventBus"></param>
        /// <param name="messageBus"></param>

        public ApartmentsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            SelectedDistrict.Building.ApartmentBuildings = new ObservableCollection<ApartmentBuilding> { };
            SelectedApartmentBuilding.PowerPlants.ElevatorsPerEntrance = new ObservableCollection<Elevator> { };
            // Получение данных о микрорайоне
            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                // присваивание присланного из DistrictMenuVM микрорайона в качестве выбранного
                SelectedDistrict = message.SharedDistrict;
            });
           


        }



        /// <summary>
        /// Отправка микрорайона с отредактированной коллекцией ApartmentBuildings
        /// </summary>
        public ICommand SendApartmentBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        /// <summary>
        /// Комманда добавления нового жилого дома
        /// </summary>
        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            ApartmentBuilding apartmentBuilding = new ApartmentBuilding();
            SelectedDistrict.Building.ApartmentBuildings.Insert(0, apartmentBuilding);
            SelectedApartmentBuilding = apartmentBuilding;
            
        }
        );

        // Флаг который отключает валидацию при копировании домов.
        bool IsCopying = false;//
        public ICommand Copy
        {
            get
            {
                return new AsyncCommand<TextBox>(async (copy) =>
                {
                   IsCopying = true;  //
                   if (SelectedApartmentBuilding != null)
                   {
                       try
                       {
                           int count = Convert.ToInt32(copy.Text);
                           int x = (from p in SelectedDistrict.Building.ApartmentBuildings.TakeWhile(p => p.Id != SelectedApartmentBuilding.Id) select p).Count();
                           while (count > 0)
                           {
                               SelectedDistrict.Building.ApartmentBuildings.Insert(x, new ApartmentBuilding());
                               SelectedDistrict.Building.ApartmentBuildings[x].ApartmentsOnSite = SelectedApartmentBuilding.ApartmentsOnSite;
                               SelectedDistrict.Building.ApartmentBuildings[x].Levels = SelectedApartmentBuilding.ApartmentsOnSite;
                               SelectedDistrict.Building.ApartmentBuildings[x].PowerPlants = SelectedApartmentBuilding.PowerPlants;
                               SelectedDistrict.Building.ApartmentBuildings[x].ElectrificationLevel = SelectedApartmentBuilding.ElectrificationLevel;
                               SelectedDistrict.Building.ApartmentBuildings[x].Entrances = SelectedApartmentBuilding.Entrances;
                               SelectedDistrict.Building.ApartmentBuildings[x].ReliabilityCathegory = SelectedApartmentBuilding.ReliabilityCathegory;
                               count--;
                           }
                       }
                       catch
                       {
                           MessageBox.Show("Введіть ціле число щоб створити копії обраної будівлі");
                       }
                 
                   }
                   IsCopying = false;
        
                });
            }
        }

        /// <summary>
        /// Комманда добавления лифта
        /// </summary>
        public ICommand AddElevatorCommand => new AsyncCommand(async () =>
        {

            Elevator elevator = new Elevator();
            SelectedApartmentBuilding.PowerPlants.GetElevators(SelectedApartmentBuilding.Entrances, elevator);
        }
       );

        /// <summary>
        ///  Комманда удаления лифта выранного
        /// </summary>
        public ICommand RemoveElevator
        {
            get
            {
                return new DelegateCommand<Elevator>((elevator) =>
                {
                    SelectedApartmentBuilding.PowerPlants.ElevatorsPerEntrance.Remove(elevator);
                    for (byte i = 0; i < SelectedApartmentBuilding.Entrances; i++)
                        SelectedApartmentBuilding.PowerPlants.Elevators.Remove(elevator);
                }, (elevator) => elevator != null);
            }
        }


        public ICommand AddPompCommand => new AsyncCommand(async () =>
        {
            Pomp pomp = new Pomp();
            SelectedApartmentBuilding.PowerPlants.Pomps.Insert(0, pomp);
        }
     );

        public ICommand RemovePomp
        {
            get
            {
                return new DelegateCommand<Pomp>((pomp) =>
                {
                    SelectedApartmentBuilding.PowerPlants.Pomps.Remove(pomp);

                }, (pomp) => pomp != null);
            }
        }

        public ICommand ExecuteCalculation => new AsyncCommand(async () =>
        {
            SelectedDistrict.CalculateApartmentBuildings();
            SelectedDistrict.Building.UniteApartmentBuildings();
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        }
        );




        /// <summary>
        /// Комманда удаления выбранного жилого дома
        /// </summary>
        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<ApartmentBuilding>((apartmentBuilding) =>
                {
                    SelectedDistrict.Building.ApartmentBuildings.Remove(apartmentBuilding);

                }, (apartmentBuilding) => apartmentBuilding != null);
            }
        }

        public void ValidationRuleApartmentBuildings(object sender, PropertyChangedEventArgs e)
       {
            if (SelectedApartmentBuilding != null)
            if (SelectedDistrict.Building.Validate(SelectedApartmentBuilding.Id, SelectedApartmentBuilding.PlanNumber)&&IsCopying!=true)
                SelectedApartmentBuilding.PlanNumber = 0;

               
        }


        /// <summary>
        /// Комманда сохранения таблицы со входными данными.
        /// </summary>
        public ICommand InitialApartmentDataToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {

                    ExportAsExcelHandler(dg, "Вхідні дані");
                });
            }
        }

        public ICommand CalculatedApartmentBuildingsToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {

                    ExportAsExcelHandler(dg, "Розрахунок будинків");
                });
            }
        }

        public ICommand UnitedApartmentBuildingsToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {
                    ExportAsExcelHandler(dg, "Будинки як один");
                });
            }
        }

        public void ExportAsExcelHandler(DataGrid dg, string worksheetsName)
        {
            string directoryName = "Excel/" + SelectedDistrict.Title + "/";
            string csvFileName = "CSV/" + "tempData.csv";
            string excelFileName = @"" + directoryName + "Житлові_будинки_мікрорайону" + ".xlsx";
            if (Directory.Exists("CSV") != true)
                Directory.CreateDirectory("CSV");
            if (Directory.Exists("Excel") != true)
                Directory.CreateDirectory("Excel");
            if (Directory.Exists(directoryName) != true)
            {
                Directory.CreateDirectory(directoryName);
            }


            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            string result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

            if (File.Exists(csvFileName))
            {
                File.Delete(csvFileName);
            }


            File.AppendAllText(csvFileName, result, Encoding.UTF8);

            bool firstRowIsHeader = false;

            var format = new ExcelTextFormat();
            format.Delimiter = ',';
            format.EOL = "\r";              // DEFAULT IS "\r\n";
            format.TextQualifier = '"';

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                try
                {
                    package.Workbook.Worksheets.Delete(worksheetsName);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    package.Save();
                }
                catch
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    package.Save();
                }

            }
            File.Delete(csvFileName);
            MessageBox.Show("Таблицю" + excelFileName + " збережено");

        }
       
        
    }


}
