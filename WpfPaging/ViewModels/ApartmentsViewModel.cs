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
        public District SelectedDistrict { get; set; } = new District();

        public ApartmentBuilding SelectedApartmentBuilding { get; set; } = new ApartmentBuilding();
        
        
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
                    for (byte i =0; i<SelectedApartmentBuilding.Entrances; i++)
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


        /// <summary>
        /// Комманда сохранения таблицы со входными данными.
        /// </summary>
        public ICommand InitialApartmentDataToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {
                    ExportData export = new ExportData();
                    export.Dg = dg;
                    export.CsvFileName = "CSV\\InitialDataApartmentBuildings.csv";
                    export.ExcelFileName = "Excel\\" + SelectedDistrict.Title + "_Вхідні_Дані_Житлові_будинки.xlsx";
                    await _messageBus.SendTo<DistrictMenuViewModel>(new ExportPathMessage(export)); 
                });
            }
        }

   
       
    }


}
